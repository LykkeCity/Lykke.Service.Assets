using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Services
{
    public interface IAssetDescriptionService
    {
        IAssetDescription CreateDefault();

        //public static AssetDescription CreateDefault(string id)
        //{
        //    return new AssetDescription
        //    {
        //        Id = id,
        //        PopIndex = 0,
        //        MarketCapitalization = string.Empty,
        //        Description = string.Empty,
        //        IssuerName = string.Empty,
        //        AssetClass = string.Empty,
        //        NumberOfCoins = string.Empty,
        //        AssetDescriptionUrl = string.Empty
        //    };
        //}
    }
}