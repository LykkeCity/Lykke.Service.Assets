using System;
using System.Linq;
using Antares.Service.Assets.Client;
using Lykke.Service.Assets.Client;
using Microsoft.Extensions.Logging;

namespace Lykke.Service.Assets.Tests.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new AssetsServiceClient("nosql.share.svc.cluster.local:5125", 
                "http://assets.lykke-service.svc.cluster.local", null);

            client.Start();


            var res = client.HttpClient.GetAssetsAsync().GetAwaiter().GetResult();

            Console.WriteLine($"Assets count: {res.Count}");

            var attr = client.AssetAttributes.GetAll().GetAwaiter().GetResult();
            Console.WriteLine($"Attr count: {attr.Count}");
            foreach (var item in attr)
            {
                Console.WriteLine($"{item.AssetId} {item.Attributes.Count()}");
            }

        }
    }
}
