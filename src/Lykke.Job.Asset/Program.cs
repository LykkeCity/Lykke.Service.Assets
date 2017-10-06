﻿using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Lykke.Job.Asset
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Asset version {Microsoft.Extensions.PlatformAbstractions.PlatformServices.Default.Application.ApplicationVersion}");
#if DEBUG
            Console.WriteLine("Is DEBUG");
#else
            Console.WriteLine("Is RELEASE");
#endif
            Console.WriteLine($"ENV_INFO: {Environment.GetEnvironmentVariable("ENV_INFO")}");

            try
            {
                var webHost = new WebHostBuilder()
                    .UseKestrel()
                    .UseUrls("http://*:5000")
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseStartup<Startup>()
                    .UseApplicationInsights()
                    .Build();

                webHost.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fatal error:");
                Console.WriteLine(ex);
            }

            Console.WriteLine("Terminated");
        }
    }
}