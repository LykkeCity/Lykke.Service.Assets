using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Repositories
{
    public class AssetsRepository : IDictionaryRepository<IAsset>
    {
        private readonly INoSQLTableStorage<AssetEntity> _tableStorage;

        public AssetsRepository(INoSQLTableStorage<AssetEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<IEnumerable<IAsset>> GetAllAsync()
        {
            var partitionKey = AssetEntity.GeneratePartitionKey();

            return (await _tableStorage.GetDataAsync(partitionKey)).Select(Asset.Create);
        }
    }
}