using Lykke.Service.Assets.Client.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.Assets.Client.Custom
{
    public interface IAssetDescription
    {
        string Id { get; set; }
        string AssetId { get; set; }
        string AssetClass { get; set; }
        string Description { get; set; }
        string IssuerName { get; set; }
        string NumberOfCoins { get; set; }
        string MarketCapitalization { get; set; }
        int PopIndex { get; set; }
        string AssetDescriptionUrl { get; set; }
        string FullName { get; set; }
    }
}
