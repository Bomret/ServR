﻿using System;
using System.Text;

namespace ServR.Example {
    class Program {
        static void Main(string[] args) {
            var http = Http.CreateServer((req, res) => {
                var hello = Encoding.UTF8.GetBytes("hello");

                res.OutputStream.Write(hello, 0, hello.Length);
            }).Listen(1337);

            Console.ReadKey();

            http.Dispose();
        }
    }
}