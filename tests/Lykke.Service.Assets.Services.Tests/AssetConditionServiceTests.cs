using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Lykke.Service.Assets.Repositories.DTOs;
using Lykke.Service.Assets.Services.Domain;

namespace Lykke.Service.Assets.Services.Tests
{
    [TestClass]
    public class AssetConditionServiceTests
    {
        private AssetConditionService _service;
        private Mock<IAssetConditionLayerRepository> _assetConditionLayerRepositoryMock;
        private Mock<IAssetConditionLayerLinkClientRepository> _assetConditionLayerLinkClientRepositoryMock;

        [TestInitialize]
        public void TestInitialized()
        {
            _assetConditionLayerRepositoryMock = new Mock<IAssetConditionLayerRepository>();
            _assetConditionLayerLinkClientRepositoryMock = new Mock<IAssetConditionLayerLinkClientRepository>();

            _service = new AssetConditionService(
                _assetConditionLayerRepositoryMock.Object,
                _assetConditionLayerLinkClientRepositoryMock.Object);
        }

        [TestMethod]
        public async Task GetAssetConditionsByClient_Ok()
        {
            // arrange
            const string clientId = "c1";
            const string asset1 = "a1";
            const string asset2 = "a2";
            const string regulation1 = "r1";
            const string regulation2 = "r2";

            var layers = new List<IAssetConditionLayer>
            {
                new AssetConditionLayerDto("1", 1, string.Empty, true, true)
                {
                    AssetConditions = new Dictionary<string, IAssetCondition>
                    {
                        { asset1, CreateAssetCondition(asset1, false, regulation1) },
                        { asset2, CreateAssetCondition(asset2, false, regulation2) }
                    }
                },
                new AssetConditionLayerDto("3", 3, string.Empty, true, true)
                {
                    AssetConditions = new Dictionary<string, IAssetCondition>
                    {
                        { asset1, CreateAssetCondition(asset1, true, null) }
                    }
                },
                new AssetConditionLayerDto("2", 2, string.Empty, true, true)
                {
                    AssetConditions = new Dictionary<string, IAssetCondition>
                    {
                        { asset1, CreateAssetCondition(asset1, null, regulation2) }
                    }
                }
            };

            _assetConditionLayerLinkClientRepositoryMock.Setup(o => o.GetAllLayersByClientAsync(It.IsAny<string>()))
                .Returns(Task.FromResult((IReadOnlyList<string>) new List<string>()));

            _assetConditionLayerRepositoryMock.Setup(o => o.GetByIdsAsync(It.IsAny<IEnumerable<string>>()))
                .Returns(Task.FromResult((IReadOnlyList<IAssetConditionLayer>) layers));

            // act
            IReadOnlyDictionary<string, IAssetCondition> result = await _service.GetAssetConditionsByClient(clientId);

            // assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(regulation2, result[asset1].Regulation);
            Assert.AreEqual(true, result[asset1].AvailableToClient);
            Assert.AreEqual(regulation2, result[asset2].Regulation);
            Assert.AreEqual(false, result[asset2].AvailableToClient);
        }

        private static AssetCondition CreateAssetCondition(string asset, bool? availableToClient, string regulation)
        {
            return new AssetCondition(asset)
            {
                AvailableToClient = availableToClient,
                Regulation = regulation
            };
        }
    }
}
