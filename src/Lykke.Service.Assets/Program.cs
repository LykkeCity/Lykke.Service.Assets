﻿using System;
using System.IO;
using System.Runtime.Loader;
using System.Threading;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Hosting;

namespace Lykke.Service.Assets
{
    [UsedImplicitly]
    internal class Program
    {
        [UsedImplicitly]
        private static void Main(string[] args)
        {
            Console.WriteLine("With SIGTERM handling");

            var webHostCancellationTokenSource = new CancellationTokenSource();
            var end = new ManualResetEvent(false);

            AssemblyLoadContext.Default.Unloading += ctx =>
            {
                Console.WriteLine("SIGTERM recieved");

                webHostCancellationTokenSource.Cancel(false);

                end.WaitOne();
            };

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseUrls("http://*:5000")
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .Build();
            
            Console.WriteLine("Running...");

            host.Run(webHostCancellationTokenSource.Token);

            Console.WriteLine("Terminating");

            end.Set();

            Console.WriteLine("Terminated");
        }
    }
}