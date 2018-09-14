using System.Collections.Generic;
using Lykke.Service.Assets.Client.Models.v3;

namespace Lykke.Service.Assets.Client.ReadModels
{
    public interface IAssetPairsReadModel
    {
        AssetPair Get(string id);
        IReadOnlyCollection<AssetPair> GetAll();
    }
}
