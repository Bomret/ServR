using System;
using System.Net;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace ServR {
    public class Http : IDisposable {
        readonly IDisposable _handler;
        readonly HttpListener _listener;

        Http(HttpListener listener, IDisposable handler) {
            _listener = listener;
            _handler = handler;
        }

        public void Dispose() {
            _listener.Stop();
            _handler.Dispose();
            _listener.Close();
        }

        public static Http Create(Action<HttpListenerRequest, HttpListenerResponse> handleRequest) {
            var listener = new HttpListener();
            listener.Start();

            var input = Observable.FromAsync(listener.GetContextAsync)
                                  .Select(c => Tuple.Create(c.Request, c.Response))
                                  .Repeat()
                                  .Retry()
                                  .Publish()
                                  .RefCount()
                                  .ObserveOn(new EventLoopScheduler())
                                  .Subscribe(t => handleRequest(t.Item1, t.Item2));

            return new Http(listener, input);
        }

        public Http Listen(int port) {
            var urlPrefix = string.Format("http://*:{0}/", port);
            _listener.Prefixes.Add(urlPrefix);

            return this;
        }
    }
}