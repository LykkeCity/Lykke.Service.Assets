using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Services.Domain;
using Microsoft.Extensions.Caching.Memory;
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
        
        private ICachedAssetConditionsService _service;
        
        private readonly AssetDefaultConditionLayer _defaultConditionLayer = new AssetDefaultConditionLayer
        {
            Id = "DefaultLayer",
            AssetConditions = new List<IAssetCondition>()
        };
        
        private readonly AssetDefaultConditionLayer _anotherConditionLayer = new AssetDefaultConditionLayer
        {
            Id = "AnotherLayer",
            AssetConditions = new List<IAssetCondition>()
        };
        
        private const int CacheTimeoutSeconds = 1;
        
        [TestInitialize]
        public void Initialize()
        {
            _service = new CachedAssetConditionsService(
                new MemoryCache(new MemoryCacheOptions()),
                TimeSpan.FromSeconds(CacheTimeoutSeconds),
                _assetDefaultConditionLayerRepositoryMock.Object,
                _assetConditionRepositoryMock.Object,
                _assetDefaultConditionRepositoryMock.Object);
        }

        [TestMethod]
        public async Task GetDefaultLayer__Cache_Expires_After_TTL()
        {
            // arrange
            MockDefaultConditionLayerTo(_defaultConditionLayer);
            
            // act
            var firstFetchResult = await _service.GetDefaultLayer();
            
            MockDefaultConditionLayerTo(_anotherConditionLayer);

            var fetchResultFromCache = await _service.GetDefaultLayer();

            // let the cache become expired
            await Task.Delay(TimeSpan.FromSeconds(CacheTimeoutSeconds + 1));
            
            var thirdFetchResult = await _service.GetDefaultLayer();

            // assert cache works fine
            Assert.AreSame(firstFetchResult, _defaultConditionLayer);
            Assert.AreSame(fetchResultFromCache, firstFetchResult);
            
            // assert cache expires correctly
            Assert.AreSame(thirdFetchResult, _anotherConditionLayer);
            Assert.AreNotSame(thirdFetchResult, firstFetchResult);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GetConditions__InvalidInput_RaisesException(string layerId)
        {
            await _service.GetConditions(layerId);
        }
        
        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GetDefaultConditions__InvalidInput_RaisesException(string layerId)
        {
            await _service.GetDefaultConditions(layerId);
        }

        private void MockDefaultConditionLayerTo(AssetDefaultConditionLayer layer)
        {
            _assetDefaultConditionLayerRepositoryMock.Setup(o => o.GetAsync())
                .Returns(Task.FromResult((IAssetDefaultConditionLayer) layer));
        }
    }
}
