using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using Lykke.Common.Log;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.NoSql.Models.AssetAttributeModel;
using Lykke.Service.Assets.Services.Domain;
using MyNoSqlServer.Abstractions;

namespace Lykke.Service.Assets.Services
{
    public class AssetAttributeService : IAssetAttributeService, IStartable
    {
        private readonly IAssetAttributeRepository _assetAttributeRepository;
        private readonly IMyNoSqlWriterWrapper<AssetAttributeNoSql> _myNoSqlWriter;

        public AssetAttributeService(
            IAssetAttributeRepository assetAttributeRepository,
            IMyNoSqlWriterWrapper<AssetAttributeNoSql> myNoSqlWriter)
        {
            _assetAttributeRepository = assetAttributeRepository;
            _myNoSqlWriter = myNoSqlWriter;
        }

        public async Task<IAssetAttribute> AddAsync(string assetId, IAssetAttribute attribute)
        {
            await _assetAttributeRepository.AddAsync(assetId, attribute);

            await _myNoSqlWriter.TryInsertOrReplaceAsync(AssetAttributeNoSql.Create(assetId, attribute.Key, attribute.Value));

            return attribute;
        }

        public async Task<IAssetAttribute> AddAsync(string assetId, string key, string value)
        {
            var attribute = new AssetAttribute
            {
                Key   = key,
                Value = value
            };

            return await AddAsync(assetId, attribute);
        }

        public async Task<IAssetAttribute> GetAsync(string assetId, string key)
        {
            return await _assetAttributeRepository.GetAsync(assetId, key);
        }

        public async Task<IEnumerable<IAssetAttributes>> GetAllAsync()
        {
            var assetIdAttributePairs = (await _assetAttributeRepository.GetAllAsync())
                .GroupBy(x => x.AssetId, x => x.Attribute)
                .Select(x => new AssetAttributes
                {
                    AssetId    = x.Key,
                    Attributes = x.Select(y => y)
                });
            
            return Mapper.Map<IEnumerable<IAssetAttributes>>(assetIdAttributePairs);
        }

        public async Task<IEnumerable<IAssetAttributes>> GetAllAsync(string assetId)
        {
            var assetAttributes = (await _assetAttributeRepository.GetAllAsync(assetId)).ToArray();

            if (assetAttributes.Any())
            {
                return new[]
                {
                    new AssetAttributes
                    {
                        AssetId    = assetId,
                        Attributes = assetAttributes
                    }
                };
            }
            else
            {
                return new AssetAttributes[0];
            }
        }

        public async Task RemoveAsync(string assetId, string key)
        {
            await _assetAttributeRepository.RemoveAsync(assetId, key);
            await _myNoSqlWriter.TryDeleteAsync(AssetAttributeNoSql.GeneratePartitionKey(assetId), AssetAttributeNoSql.GenerateRowKey(key));
        }

        public async Task UpdateAsync(string assetId, IAssetAttribute attribute)
        {
            await _assetAttributeRepository.UpdateAsync(assetId, attribute);
            await _myNoSqlWriter.TryInsertOrReplaceAsync(AssetAttributeNoSql.Create(assetId, attribute.Key, attribute.Value));
        }
        
        public async Task UpdateAsync(string assetId, string key, string value)
        {
            await UpdateAsync(assetId, new AssetAttribute { Key = key, Value = value });
        }

        private IList<AssetAttributeNoSql> ReadAllData()
        {
            var records = _assetAttributeRepository.GetAllAsync().GetAwaiter().GetResult();
            var data = records.Select(e => AssetAttributeNoSql.Create(e.AssetId, e.Attribute.Key, e.Attribute.Value)).ToList();
            return data;
        }

        public void Start()
        {
            _myNoSqlWriter.Start(ReadAllData);
        }
    }
}
