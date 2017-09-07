using Lykke.Service.Assets.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using AzureStorage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.Assets.Repositories
{
    public class IssuerEntity : TableEntity, IIssuer
    {

        public static string GeneratePartitionKey()
        {
            return "Issuer";
        }

        public static string GenerateRowKey(string id)
        {
            return id;
        }

        public string Id => RowKey;
        public string Name { get; set; }
        public string IconUrl { get; set; }

        public static IssuerEntity Create(IIssuer issuer)
        {
            return new IssuerEntity
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = GenerateRowKey(issuer.Id),
                Name = issuer.Name,
                IconUrl = issuer.IconUrl
            };
        }

    }

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
