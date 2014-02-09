using System;
using System.Net;

namespace ServR.Task {
    public class HttpServer : IDisposable {
        readonly HttpListener _listener;

        public HttpServer(HttpListener listener) {
            _listener = listener;
        }

        public void Dispose() {
            _listener.Stop();
            _listener.Close();
        }

        public async void Listen(
            Func<HttpListenerRequest, HttpListenerResponse, System.Threading.Tasks.Task> onRequest) {
            _listener.Start();

            while (_listener.IsListening) {
                var context = await _listener.GetContextAsync();

                try {
                    await onRequest(context.Request, context.Response);
                }
                catch (Exception error) {
                    Console.WriteLine(error.Message);

                    context.Response.StatusCode = 500;
                }
                finally {
                    context.Response.Close();
                }
            }
        }
    }
}