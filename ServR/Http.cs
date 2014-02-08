﻿using System;
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
                                  .Select(c => Tuple.Create(c.Request, c.Response))
                                  .Repeat()
                                  .Retry()
                                  .Publish()
                                  .RefCount()
                                  .ObserveOn(scheduler ?? new EventLoopScheduler())
                                  .Subscribe(t => handleRequest(t.Item1, t.Item2));

            return new HttpServer(listener, input);
        }
    }
}