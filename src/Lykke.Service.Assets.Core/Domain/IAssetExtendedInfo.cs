using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.Service.Assets.Core.Domain
{
    public interface IAssetExtendedInfo : IDictionaryItem
    {
        string Id { get; }
        string AssetClass { get; }
        string Description { get; }
        string IssuerName { get; set; }
        string NumberOfCoins { get; }
        string MarketCapitalization { get; }
        int PopIndex { get; }
        string AssetDescriptionUrl { get; }
        string FullName { get; }
    }

    public class AssetExtendedInfo : IAssetExtendedInfo
    {
        public string Id { get; set; }
        public string AssetClass { get; set; }
        public string Description { get; set; }
        public string IssuerName { get; set; }
        public string NumberOfCoins { get; set; }
        public string MarketCapitalization { get; set; }
        public int PopIndex { get; set; }
        public string AssetDescriptionUrl { get; set; }
        public string FullName { get; set; }


        public static AssetExtendedInfo CreateDefault(string id)
        {
            return new AssetExtendedInfo
            {
                Id = id,
                PopIndex = 0,
                MarketCapitalization = string.Empty,
                Description = string.Empty,
                IssuerName = string.Empty,
                AssetClass = string.Empty,
                NumberOfCoins = string.Empty,
                AssetDescriptionUrl = string.Empty
            };
        }

        public static AssetExtendedInfo Create(IAssetExtendedInfo src, IIssuer issuer)
        {
            return new AssetExtendedInfo
            {
                Id = src.Id,
                AssetClass = src.AssetClass,
                Description = src.Description,
                IssuerName = issuer?.Name,
                MarketCapitalization = src.MarketCapitalization,
                NumberOfCoins = src.NumberOfCoins,
                PopIndex = src.PopIndex,
                AssetDescriptionUrl = src.AssetDescriptionUrl,
                FullName = src.FullName
            };
        }
    }

    public interface IAssetExtendedInfoRepository
    {

        Task SaveAsync(IAssetExtendedInfo src);
        Task<IAssetExtendedInfo> GetAssetExtendedInfoAsync(string id);
        Task<IEnumerable<IAssetExtendedInfo>> GetAllAsync();
    }


    public static class AssetExtendedInfoExt
    {
        public static async Task<IAssetExtendedInfo> GetAssetExtendedInfoOrDefaultAsync(this IAssetExtendedInfoRepository table, string id)
        {
            if (id == null)
                return AssetExtendedInfo.CreateDefault(null);
            var aei = await table.GetAssetExtendedInfoAsync(id);
            return aei ?? AssetExtendedInfo.CreateDefault(null);
        }
    }
}
