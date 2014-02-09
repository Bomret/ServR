using System;
using System.Text;

namespace ServR.Task.Example {
    class Program {
        static void Main(string[] args) {
            var server = Http.CreateServer(1337);

            server.Listen(async (req, res) => {
                var hello = Encoding.UTF8.GetBytes("hello");

                await res.OutputStream.WriteAsync(hello, 0, hello.Length);
                res.Close();
            });

            Console.ReadKey();

            server.Dispose();
        }
    }
}