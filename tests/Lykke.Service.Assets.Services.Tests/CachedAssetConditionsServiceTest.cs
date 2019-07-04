using System;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Repositories.DTOs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Lykke.Service.Assets.Services.Tests
{
    [TestClass]
    public class CachedAssetConditionsServiceTest
    {
        private readonly Mock<IAssetDefaultConditionLayerRepository> _assetDefaultConditionLayerRepositoryMock =
            new Mock<IAssetDefaultConditionLayerRepository>();

        private readonly Mock<IAssetConditionRepository> _assetConditionRepositoryMock =
            new Mock<IAssetConditionRepository>();

        private readonly Mock<IAssetDefaultConditionRepository> _assetDefaultConditionRepositoryMock =
            new Mock<IAssetDefaultConditionRepository>();

        private readonly Mock<IDistributedCache<IAssetDefaultConditionLayer, AssetDefaultConditionLayerDto>>_assetDefaultConditionLayerCache =
            new Mock<IDistributedCache<IAssetDefaultConditionLayer, AssetDefaultConditionLayerDto>>();

        private readonly Mock<IDistributedCache<IAssetCondition, AssetConditionDto>> _assetConditionCache =
            new Mock<IDistributedCache<IAssetCondition, AssetConditionDto>>();

        private readonly Mock<IDistributedCache<IAssetDefaultCondition, AssetDefaultConditionDto>> _assetDefaultConditionCache =
            new Mock<IDistributedCache<IAssetDefaultCondition, AssetDefaultConditionDto>>();

        private readonly Mock<IAssetsForClientCacheManager> _assetsForClientCacheManagerMock = new Mock<IAssetsForClientCacheManager>();

        private ICachedAssetConditionsService _service;

        [TestInitialize]
        public void Initialize()
        {
            _service = new CachedAssetConditionsService(
                _assetDefaultConditionLayerRepositoryMock.Object,
                _assetConditionRepositoryMock.Object,
                _assetDefaultConditionRepositoryMock.Object,
                _assetDefaultConditionLayerCache.Object,
                _assetConditionCache.Object,
                _assetDefaultConditionCache.Object,
                _assetsForClientCacheManagerMock.Object
                );
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GetConditions__InvalidInput_RaisesException(string layerId)
        {
            await _service.GetConditionsAsync(layerId);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GetDefaultConditions__InvalidInput_RaisesException(string layerId)
        {
            await _service.GetDefaultConditionsAsync(layerId);
        }
    }
}
