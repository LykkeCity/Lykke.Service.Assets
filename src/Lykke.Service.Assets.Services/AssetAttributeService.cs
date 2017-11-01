using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Services.Domain;

namespace Lykke.Service.Assets.Services
{
    public class AssetAttributeService : IAssetAttributeService
    {
        private readonly IAssetAttributeRepository _assetAttributeRepository;

        public AssetAttributeService(
            IAssetAttributeRepository assetAttributeRepository)
        {
            _assetAttributeRepository = assetAttributeRepository;
        }

        public async Task<IAssetAttribute> AddAsync(string assetId, IAssetAttribute attribute)
        {
            await _assetAttributeRepository.AddAsync(assetId, attribute);

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
        }

        public async Task UpdateAsync(string assetId, IAssetAttribute attribute)
        {
            await _assetAttributeRepository.UpdateAsync(assetId, attribute);
        }
        
        public async Task UpdateAsync(string assetId, string key, string value)
        {
            await UpdateAsync(assetId, new AssetAttribute { Key = key, Value = value });
        }
    }
}