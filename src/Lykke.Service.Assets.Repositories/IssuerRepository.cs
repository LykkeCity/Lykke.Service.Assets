using Lykke.Service.Assets.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Repositories.Entities;

namespace Lykke.Service.Assets.Repositories
{
    public class IssuerRepository : IIssuerRepository, IDictionaryRepository<IIssuer>
    {
        private readonly INoSQLTableStorage<IssuerEntity> _tableStorage;

        public IssuerRepository(INoSQLTableStorage<IssuerEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public Task RegisterIssuerAsync(IIssuer issuer)
        {
            var newIssuer = IssuerEntity.Create(issuer);
            return _tableStorage.InsertAsync(newIssuer);
        }

        public async Task EditIssuerAsync(string id, IIssuer issuer)
        {
            await _tableStorage.DeleteAsync(IssuerEntity.GeneratePartitionKey(), IssuerEntity.GenerateRowKey(id));
            await RegisterIssuerAsync(issuer);
        }

        public async Task<IEnumerable<IIssuer>> GetAllIssuersAsync()
        {
            var partitionKey = IssuerEntity.GeneratePartitionKey();
            return await _tableStorage.GetDataAsync(partitionKey);
        }

        public async Task<IIssuer> GetIssuerAsync(string id)
        {
            var partitionKey = IssuerEntity.GeneratePartitionKey();
            var rowKey = IssuerEntity.GenerateRowKey(id);

            return await _tableStorage.GetDataAsync(partitionKey, rowKey);
        }

        public async Task<IEnumerable<IIssuer>> GetAllAsync()
        {
            return await GetAllIssuersAsync();
        }
    }
}
