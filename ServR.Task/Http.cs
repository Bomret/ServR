using System.Net;

namespace ServR.Task {
    public static class Http {
        public static HttpServer CreateServer(int port) {
            var listener = new HttpListener();
            var urlPrefix = string.Format("http://*:{0}/", port);
            listener.Prefixes.Add(urlPrefix);

            return new HttpServer(listener);
        }
    }
}