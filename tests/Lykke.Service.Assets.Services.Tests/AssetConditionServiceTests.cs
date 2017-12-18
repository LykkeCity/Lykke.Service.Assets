using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Lykke.Service.Assets.Repositories.DTOs;
using Lykke.Service.Assets.Services.Domain;

namespace Lykke.Service.Assets.Services.Tests
{
    [TestClass]
    public class AssetConditionServiceTests : Test
    {
        private AssetConditionService _service;
        private Mock<IAssetRepository> _assetRepositoryMock;
        private Mock<IAssetConditionLayerRepository> _assetConditionLayerRepositoryMock;
        private Mock<IAssetConditionLayerLinkClientRepository> _assetConditionLayerLinkClientRepositoryMock;
        private Mock<IAssetConditionDefaultLayerRepository> _assetConditionDefaultLayerRepositoryMock;
        private Mock<IAssetsForClientCacheManager> _cacheMock;

        [TestInitialize]
        public void TestInitialized()
        {
            _assetRepositoryMock = new Mock<IAssetRepository>();
            _assetConditionLayerRepositoryMock = new Mock<IAssetConditionLayerRepository>();
            _assetConditionLayerLinkClientRepositoryMock = new Mock<IAssetConditionLayerLinkClientRepository>();
            _assetConditionDefaultLayerRepositoryMock = new Mock<IAssetConditionDefaultLayerRepository>();
            _cacheMock = new Mock<IAssetsForClientCacheManager>();

            _service = new AssetConditionService(
                _assetRepositoryMock.Object,
                _assetConditionLayerRepositoryMock.Object,
                _assetConditionLayerLinkClientRepositoryMock.Object,
                _assetConditionDefaultLayerRepositoryMock.Object,
                _cacheMock.Object);
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

        [TestMethod]
        public async Task GetAssetConditionsByClient_Use_Default_Settings_If_No_Conditions()
        {
            // arrange
            const string clientId = "client1";
            const string assetId = "asset1";

            var defaultLayer = new AssetConditionDefaultLayer
            {
                AvailableToClient = true,
                Regulation = "regulation1"
            };

            _assetRepositoryMock.Setup(o => o.GetAllAsync(It.IsAny<bool>()))
                .Returns(Task.FromResult((IEnumerable<IAsset>)new[] { new Asset{Id = assetId} }));

            _assetConditionLayerLinkClientRepositoryMock.Setup(o => o.GetAllLayersByClientAsync(It.IsAny<string>()))
                .Returns(Task.FromResult((IReadOnlyList<string>)new List<string>()));

            _assetConditionLayerRepositoryMock.Setup(o => o.GetByIdsAsync(It.IsAny<IEnumerable<string>>()))
                .Returns(Task.FromResult((IReadOnlyList<IAssetConditionLayer>) new List<IAssetConditionLayer>()));

            _assetConditionDefaultLayerRepositoryMock.Setup(o => o.GetAsync())
                .Returns(Task.FromResult((IAssetConditionDefaultLayer) defaultLayer));

            // act
            IReadOnlyDictionary<string, IAssetCondition> conditions = await _service.GetAssetConditionsByClient(clientId);

            // assert
            Assert.IsTrue(conditions.ContainsKey(assetId));
            Assert.AreEqual(defaultLayer.Regulation, conditions[assetId].Regulation);
            Assert.AreEqual(defaultLayer.AvailableToClient, conditions[assetId].AvailableToClient);
        }

        [TestMethod]
        public async Task GetAssetConditionsByClient_Use_Default_Settings_If_Conditions_Undefined()
        {
            // arrange
            const string clientId = "client1";
            const string assetId = "asset1";

            var defaultLayer = new AssetConditionDefaultLayer
            {
                AvailableToClient = true,
                Regulation = "regulation1"
            };

            var layers = new List<IAssetConditionLayer>
            {
                new AssetConditionLayerDto("1", 1, string.Empty, true, true)
                {
                    AssetConditions = new Dictionary<string, IAssetCondition>
                    {
                        { assetId, CreateAssetCondition(assetId, null, null) }
                    }
                }
            };

            _assetRepositoryMock.Setup(o => o.GetAllAsync(It.IsAny<bool>()))
                .Returns(Task.FromResult((IEnumerable<IAsset>)new[] { new Asset { Id = assetId } }));

            _assetConditionLayerLinkClientRepositoryMock.Setup(o => o.GetAllLayersByClientAsync(It.IsAny<string>()))
                .Returns(Task.FromResult((IReadOnlyList<string>)new List<string>()));

            _assetConditionLayerRepositoryMock.Setup(o => o.GetByIdsAsync(It.IsAny<IEnumerable<string>>()))
                .Returns(Task.FromResult((IReadOnlyList<IAssetConditionLayer>)layers));

            _assetConditionDefaultLayerRepositoryMock.Setup(o => o.GetAsync())
                .Returns(Task.FromResult((IAssetConditionDefaultLayer)defaultLayer));

            // act
            IReadOnlyDictionary<string, IAssetCondition> conditions = await _service.GetAssetConditionsByClient(clientId);

            // assert
            Assert.IsTrue(conditions.ContainsKey(assetId));
            Assert.AreEqual(defaultLayer.Regulation, conditions[assetId].Regulation);
            Assert.AreEqual(defaultLayer.AvailableToClient, conditions[assetId].AvailableToClient);
        }

        [TestMethod]
        public async Task GetAssetConditionsByClient_Override_Default_Settings()
        {
            // arrange
            const string clientId = "client1";
            const string assetId = "asset1";
            const string regulation1 = "regulation1";
            const string regulation2 = "regulation2";

            var defaultLayer = new AssetConditionDefaultLayer
            {
                AvailableToClient = false,
                Regulation = regulation1
            };

            var layers = new List<IAssetConditionLayer>
            {
                new AssetConditionLayerDto("1", 1, string.Empty, true, true)
                {
                    AssetConditions = new Dictionary<string, IAssetCondition>
                    {
                        { assetId, CreateAssetCondition(assetId, true, regulation2) }
                    }
                }
            };

            _assetRepositoryMock.Setup(o => o.GetAllAsync(It.IsAny<bool>()))
                .Returns(Task.FromResult((IEnumerable<IAsset>)new[] { new Asset { Id = assetId } }));

            _assetConditionLayerLinkClientRepositoryMock.Setup(o => o.GetAllLayersByClientAsync(It.IsAny<string>()))
                .Returns(Task.FromResult((IReadOnlyList<string>)new List<string>()));

            _assetConditionLayerRepositoryMock.Setup(o => o.GetByIdsAsync(It.IsAny<IEnumerable<string>>()))
                .Returns(Task.FromResult((IReadOnlyList<IAssetConditionLayer>)layers));

            _assetConditionDefaultLayerRepositoryMock.Setup(o => o.GetAsync())
                .Returns(Task.FromResult((IAssetConditionDefaultLayer)defaultLayer));

            // act
            IReadOnlyDictionary<string, IAssetCondition> conditions = await _service.GetAssetConditionsByClient(clientId);

            // assert
            Assert.IsTrue(conditions.ContainsKey(assetId));
            Assert.AreEqual(regulation2, conditions[assetId].Regulation);
            Assert.AreEqual(true, conditions[assetId].AvailableToClient);
        }

        [TestMethod]
        public async Task GetAssetConditionsLayerSettingsByClient_Use_Default_Settings_If_No_Conditions()
        {
            // arrange
            const string clientId = "client1";

            var defaultLayer = new AssetConditionDefaultLayer
            {
                SwiftDepositEnabled = true,
                ClientsCanCashInViaBankCards = false
            };

            var layers = new List<IAssetConditionLayer>();

            _assetConditionLayerRepositoryMock.Setup(o => o.GetByIdsAsync(It.IsAny<IEnumerable<string>>()))
                .Returns(Task.FromResult((IReadOnlyList<IAssetConditionLayer>)layers));

            _assetConditionDefaultLayerRepositoryMock.Setup(o => o.GetAsync())
                .Returns(Task.FromResult((IAssetConditionDefaultLayer)defaultLayer));

            // act
            IAssetConditionLayerSettings settings = await _service.GetAssetConditionsLayerSettingsByClient(clientId);

            // assert
            Assert.AreEqual(defaultLayer.ClientsCanCashInViaBankCards, settings.ClientsCanCashInViaBankCards);
            Assert.AreEqual(defaultLayer.SwiftDepositEnabled, settings.SwiftDepositEnabled);
        }

        [TestMethod]
        public async Task GetAssetConditionsLayerSettingsByClient_Use_Default_Settings_If_Conditions_Undefined()
        {
            // arrange
            const string clientId = "client1";

            var defaultLayer = new AssetConditionDefaultLayer
            {
                SwiftDepositEnabled = true,
                ClientsCanCashInViaBankCards = false
            };

            var layers = new List<IAssetConditionLayer>
            {
                new AssetConditionLayerDto("1", 1, string.Empty, null, null)
            };

            _assetConditionLayerRepositoryMock.Setup(o => o.GetByIdsAsync(It.IsAny<IEnumerable<string>>()))
                .Returns(Task.FromResult((IReadOnlyList<IAssetConditionLayer>)layers));

            _assetConditionDefaultLayerRepositoryMock.Setup(o => o.GetAsync())
                .Returns(Task.FromResult((IAssetConditionDefaultLayer)defaultLayer));

            // act
            IAssetConditionLayerSettings settings = await _service.GetAssetConditionsLayerSettingsByClient(clientId);

            // assert
            Assert.AreEqual(defaultLayer.ClientsCanCashInViaBankCards, settings.ClientsCanCashInViaBankCards);
            Assert.AreEqual(defaultLayer.SwiftDepositEnabled, settings.SwiftDepositEnabled);
        }

        [TestMethod]
        public async Task GetAssetConditionsLayerSettingsByClient_Override_Default_Settings()
        {
            // arrange
            const string clientId = "client1";

            var defaultLayer = new AssetConditionDefaultLayer
            {
                SwiftDepositEnabled = true,
                ClientsCanCashInViaBankCards = false
            };

            var layers = new List<IAssetConditionLayer>
            {
                new AssetConditionLayerDto("1", 1, string.Empty, true, false)
            };

            _assetConditionLayerRepositoryMock.Setup(o => o.GetByIdsAsync(It.IsAny<IEnumerable<string>>()))
                .Returns(Task.FromResult((IReadOnlyList<IAssetConditionLayer>)layers));

            _assetConditionDefaultLayerRepositoryMock.Setup(o => o.GetAsync())
                .Returns(Task.FromResult((IAssetConditionDefaultLayer)defaultLayer));

            // act
            IAssetConditionLayerSettings settings = await _service.GetAssetConditionsLayerSettingsByClient(clientId);

            // assert
            Assert.AreEqual(true, settings.ClientsCanCashInViaBankCards);
            Assert.AreEqual(false, settings.SwiftDepositEnabled);
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
