using Lykke.Service.Assets.Core.Services;

namespace Lykke.Service.Assets.Services
{
    public class IssuerService : IIssuerService
    {
        private static string GetPartitionKey()
            => "Issuer";

        private static string GetRowKey(string id)
            => id;
    }
}