using System.Collections.Generic;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Repositories.Entities;


namespace Lykke.Service.Assets.Repositories
{
    public class AssetRepository : IDictionaryRepository<IAsset>
    {
        private readonly INoSQLTableStorage<AssetEntity> _tableStorage;


        public AssetRepository(INoSQLTableStorage<AssetEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }


        public async Task<IEnumerable<IAsset>> GetAllAsync()
        {
            return await _tableStorage.GetDataAsync();
        }
    }
}