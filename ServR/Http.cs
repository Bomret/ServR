using System;
using System.Net;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace ServR {
    public static class Http {
        public static HttpServer CreateServer(Action<HttpListenerRequest, HttpListenerResponse> handleRequest,
                                              IScheduler scheduler = null) {
            var listener = new HttpListener();
            listener.Start();

            var input = Observable.FromAsync(listener.GetContextAsync)
                                  .Retry()
                                  .Repeat()
                                  .Publish()
                                  .RefCount()
                                  .ObserveOn(scheduler ?? new EventLoopScheduler())
                                  .Subscribe(ctx => {
                                      try {
                                          handleRequest(ctx.Request, ctx.Response);
                                      }
                                      catch (Exception error) {
                                          ctx.Response.StatusCode = 500;
                                      }
                                      finally {
                                          ctx.Response.Close();
                                      }
                                  });

            return new HttpServer(listener, input);
        }
    }
}