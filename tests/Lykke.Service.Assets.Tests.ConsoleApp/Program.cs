using System;
using System.Linq;
using System.Threading;
using Antares.Service.Assets.Client;
using Lykke.Service.Assets.Client;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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

            Console.WriteLine($"Assets tradable count: {res.Count}");

            var attr = client.AssetAttributes.GetAll();
            Console.WriteLine($"Attr count: {attr.Count}");
            foreach (var item in attr)
            {
                Console.WriteLine($"{item.AssetId} {item.Attributes.Count()}");
            }

            var info = client.AssetExtendedInfo.Get("BTC");
            Console.WriteLine(JsonConvert.SerializeObject(info));

            Console.WriteLine();
            Console.WriteLine("BTC ASSET");
            var asset = client.Assets.Get("BTC");
            Console.WriteLine(JsonConvert.SerializeObject(asset));


            Console.WriteLine();
            Console.WriteLine("spec 1");
            var assets = client.Assets.GetBySpecification(ids: new [] {"BTC", "ETH"});
            Console.WriteLine($"count: {assets.Count}");

            Console.WriteLine();
            Console.WriteLine("spec 2");
            assets = client.Assets.GetBySpecification(IsTradable: true);
            Console.WriteLine($"count IsTradable: {assets.Count}");

            Console.WriteLine();
            Console.WriteLine("spec 3");
            assets = client.Assets.GetBySpecification(IsTradable: false);
            Console.WriteLine($"count Not IsTradable: {assets.Count}");
        }
    }
}
