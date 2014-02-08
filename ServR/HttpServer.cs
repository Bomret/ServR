using System;
using System.Net;

namespace ServR {
    public class HttpServer : IDisposable {
        readonly IDisposable _handleRequest;
        readonly HttpListener _listener;

        internal HttpServer(HttpListener listener, IDisposable handleRequest) {
            _listener = listener;
            _handleRequest = handleRequest;
        }

        public void Dispose() {
            _listener.Stop();
            _handleRequest.Dispose();
            _listener.Close();
        }

        public HttpServer Listen(int port) {
            var urlPrefix = string.Format("http://*:{0}/", port);
            _listener.Prefixes.Add(urlPrefix);

            return this;
        }
    }
}