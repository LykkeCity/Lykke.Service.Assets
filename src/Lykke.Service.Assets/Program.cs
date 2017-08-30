using System;
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
            Console.WriteLine($"Assets version {Microsoft.Extensions.PlatformAbstractions.PlatformServices.Default.Application.ApplicationVersion}");
#if DEBUG
            Console.WriteLine("Is DEBUG");
#else
            Console.WriteLine("Is RELEASE");
#endif   

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseUrls("http://*:5000")
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .Build();
            
            host.Run();

            Console.WriteLine("Terminated");
        }
    }
}