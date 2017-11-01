using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using AzureStorage;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Repositories.Entities;


namespace Lykke.Service.Assets.Repositories
{
    public class MarginIssuerRepository : IMarginIssuerRepository
    {
        private readonly INoSQLTableStorage<MarginIssuerEntity> _marginIssuerTable;


        public MarginIssuerRepository(
            INoSQLTableStorage<MarginIssuerEntity> marginIssuerTable)
        {
            _marginIssuerTable = marginIssuerTable;
        }


        public async Task AddAsync(IMarginIssuer marginIssuer)
        {
            var entity = Mapper.Map<MarginIssuerEntity>(marginIssuer);

            entity.PartitionKey = GetPartitionKey();
            entity.RowKey       = GetRowKey(marginIssuer.Id);

            await _marginIssuerTable.InsertAsync(entity);
        }

        public async Task<IEnumerable<IMarginIssuer>> GetAllAsync()
        {
            return await _marginIssuerTable.GetDataAsync(GetPartitionKey());
        }

        public async Task<IMarginIssuer> GetAsync(string id)
        {
            return await _marginIssuerTable.GetDataAsync(GetPartitionKey(), GetRowKey(id));
        }

        public async Task RemoveAsync(string id)
        {
            await _marginIssuerTable.DeleteIfExistAsync(GetPartitionKey(), GetRowKey(id));
        }

        public async Task UpdateAsync(IMarginIssuer marginIssuer)
        {
            await _marginIssuerTable.MergeAsync(GetPartitionKey(), GetRowKey(marginIssuer.Id), x =>
            {
                Mapper.Map(marginIssuer, x);

                return x;
            });
        }

        private static string GetPartitionKey()
            => "MarginIssuer";

        private static string GetRowKey(string id)
            => id;
    }
}