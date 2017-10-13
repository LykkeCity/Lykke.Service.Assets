using Lykke.Service.Assets.Core.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AzureStorage;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Repositories.Entities;

namespace Lykke.Service.Assets.Repositories
{
    public class IssuerRepository : IIssuerRepository
    {
        private readonly INoSQLTableStorage<IssuerEntity> _issuerTable;


        public IssuerRepository(
            INoSQLTableStorage<IssuerEntity> issuerTable)
        {
            _issuerTable = issuerTable;
        }

        public async Task AddAsync(IIssuer issuer)
        {
            var entity = Mapper.Map<IssuerEntity>(issuer);

            entity.PartitionKey = GetPartitionKey();
            entity.RowKey       = GetRowKey(issuer.Id);

            await _issuerTable.InsertAsync(entity);
        }

        public async Task<IEnumerable<IIssuer>> GetAllAsync()
        {
            return await _issuerTable.GetDataAsync(GetPartitionKey());
        }

        public async Task<IIssuer> GetAsync(string id)
        {
            return await _issuerTable.GetDataAsync(GetPartitionKey(), GetRowKey(id));
        }

        public async Task<IEnumerable<IIssuer>> GetAsync(IEnumerable<string> ids)
        {
            return await _issuerTable.GetDataAsync(GetPartitionKey(), ids.Select(GetRowKey));
        }

        public async Task RemoveAsync(string id)
        {
            await _issuerTable.DeleteIfExistAsync(GetPartitionKey(), GetRowKey(id));
        }

        public async Task UpdateAsync(IIssuer issuer)
        {
            await _issuerTable.ReplaceAsync(GetPartitionKey(), GetRowKey(issuer.Id), x =>
            {
                Mapper.Map(issuer, x);

                return x;
            });
        }

        private static string GetPartitionKey()
            => "Issuer";

        private static string GetRowKey(string id)
            => id;
    }
}
