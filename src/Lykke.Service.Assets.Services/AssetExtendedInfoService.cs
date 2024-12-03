using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.NoSql;
using Lykke.Service.Assets.NoSql.Models;
using Lykke.Service.Assets.Services.Domain;


namespace Lykke.Service.Assets.Services
{
    public class AssetExtendedInfoService : IAssetExtendedInfoService, IStartable
    {
        private readonly IAssetExtendedInfoRepository _assetExtendedInfoRepository;
        private readonly IMyNoSqlWriterWrapper<AssetExtendedInfoNoSql> _myNoSqlWriter;

        public AssetExtendedInfoService(
            IAssetExtendedInfoRepository assetExtendedInfoRepository,
            IMyNoSqlWriterWrapper<AssetExtendedInfoNoSql> myNoSqlWriter)
        {
            _assetExtendedInfoRepository = assetExtendedInfoRepository;
            _myNoSqlWriter = myNoSqlWriter;
        }

        public async Task<IAssetExtendedInfo> AddAsync(IAssetExtendedInfo assetInfo)
        {
            await _assetExtendedInfoRepository.UpsertAsync(assetInfo);
            await _myNoSqlWriter.TryInsertOrReplaceAsync(AssetExtendedInfoNoSql.Create(assetInfo));

            return assetInfo;
        }

        public IAssetExtendedInfo CreateDefault()
        {
            return new AssetExtendedInfo
            {
                Id                   = string.Empty,
                PopIndex             = 0,
                MarketCapitalization = string.Empty,
                Description          = string.Empty,
                AssetClass           = string.Empty,
                NumberOfCoins        = string.Empty,
                AssetDescriptionUrl  = string.Empty
            };
        }

        public async Task<IEnumerable<IAssetExtendedInfo>> GetAllAsync()
        {
            return await _assetExtendedInfoRepository.GetAllAsync();
        }

        public async Task<IAssetExtendedInfo> GetAsync(string id)
        {
            return await _assetExtendedInfoRepository.GetAsync(id);
        }

        public async Task RemoveAsync(string id)
        {
            await _assetExtendedInfoRepository.RemoveAsync(id);
            await _myNoSqlWriter.TryDeleteAsync(AssetExtendedInfoNoSql.GeneratePartitionKey(), AssetExtendedInfoNoSql.GenerateRowKey(id));
        }

        public async Task UpdateAsync(IAssetExtendedInfo assetInfo)
        {
            await _assetExtendedInfoRepository.UpsertAsync(assetInfo);
            await _myNoSqlWriter.TryInsertOrReplaceAsync(AssetExtendedInfoNoSql.Create(assetInfo));
        }

        public void Start()
        {
            _myNoSqlWriter.Start(ReadAllData);
        }

        private IList<AssetExtendedInfoNoSql> ReadAllData()
        {
            var data = GetAllAsync().GetAwaiter().GetResult().Select(AssetExtendedInfoNoSql.Create).ToList();
            var defaultItem = AssetExtendedInfoNoSql.Create(CreateDefault());
            defaultItem.PartitionKey = AssetExtendedInfoNoSql.DefaultInfoPartitionKey;
            defaultItem.RowKey = AssetExtendedInfoNoSql.DefaultInfoRowKey;

            data.Add(defaultItem);

            return data;
        }
    }
}
