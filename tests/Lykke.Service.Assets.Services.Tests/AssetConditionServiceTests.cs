using System.Collections.Generic;
using System.Linq;
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
    public class AssetConditionServiceTests
    {
        private readonly List<IAssetConditionLayer> _layers = new List<IAssetConditionLayer>();

        private readonly Mock<IAssetConditionRepository> _assetConditionRepositoryMock =
            new Mock<IAssetConditionRepository>();

        private readonly Mock<IAssetConditionSettingsRepository> _assetConditionSettingsRepositoryMock =
            new Mock<IAssetConditionSettingsRepository>();

        private readonly Mock<IAssetConditionLayerSettingsRepository> _assetConditionLayerSettingsRepositoryMock =
            new Mock<IAssetConditionLayerSettingsRepository>();

        private readonly Mock<IAssetConditionLayerRepository> _assetConditionLayerRepositoryMock =
            new Mock<IAssetConditionLayerRepository>();

        private readonly Mock<IAssetConditionLayerLinkClientRepository> _assetConditionLayerLinkClientRepositoryMock =
            new Mock<IAssetConditionLayerLinkClientRepository>();

        private readonly Mock<IAssetsForClientCacheManager> _cacheMock =
            new Mock<IAssetsForClientCacheManager>();

        private AssetConditionService _service;

        [TestInitialize]
        public void TestInitialized()
        {
            _service = new AssetConditionService(
                _assetConditionRepositoryMock.Object,
                _assetConditionLayerRepositoryMock.Object,
                _assetConditionSettingsRepositoryMock.Object,
                _assetConditionLayerSettingsRepositoryMock.Object,
                _assetConditionLayerLinkClientRepositoryMock.Object,
                _cacheMock.Object);

            _assetConditionLayerLinkClientRepositoryMock.Setup(o => o.GetAllLayersByClientAsync(It.IsAny<string>()))
                .Returns(Task.FromResult((IReadOnlyList<string>) new List<string>()));

            _assetConditionLayerRepositoryMock.Setup(o => o.GetAsync(It.IsAny<IEnumerable<string>>()))
                .Returns(Task.FromResult((IEnumerable<IAssetConditionLayer>) _layers));

            _assetConditionRepositoryMock.Setup(o => o.GetAsync(It.IsAny<string>()))
                .Returns((string layerId) =>Task.FromResult((IEnumerable<IAssetCondition>) _layers.First(o => o.Id == layerId).AssetConditions));

            _assetConditionLayerSettingsRepositoryMock.Setup(o => o.GetAsync())
                .Returns(Task.FromResult((IAssetConditionLayerSettings) new AssetConditionLayerSettings()));

            _assetConditionSettingsRepositoryMock.Setup(o => o.GetAsync())
                .Returns(Task.FromResult((IAssetConditionSettings) new AssetConditionSettings()));
        }

        [TestMethod]
        public async Task GetAssetConditionsByClient_Valid_Count()
        {
            // arrange
            const string clientId = "c1";
            const string asset1 = "a1";
            const string asset2 = "a2";
            const string regulation1 = "r1";
            const string regulation2 = "r2";

            _layers.AddRange(new[]
            {
                CreateAssetConditionLayer("1", 1, true, true, new List<IAssetCondition>
                {
                    CreateAssetCondition(asset1, false, regulation1),
                    CreateAssetCondition(asset2, false, regulation2)
                }),
                CreateAssetConditionLayer("3", 3, true, true, new List<IAssetCondition>
                {
                    CreateAssetCondition(asset1, true, null)
                }),
                CreateAssetConditionLayer("2", 2, true, true, new List<IAssetCondition>
                {
                    CreateAssetCondition(asset1, null, regulation2)
                })
            });
            
            // act
            IEnumerable<IAssetCondition> result = await _service.GetAssetConditionsByClient(clientId);

            // assert
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public async Task GetAssetConditionsByClient_Valid_Merge()
        {
            // arrange
            const string clientId = "c1";
            const string asset1 = "a1";
            const string regulation1 = "r1";
            const string regulation2 = "r2";

            _layers.AddRange(new[]
            {
                CreateAssetConditionLayer("1", 1, true, true, new List<IAssetCondition>
                {
                    CreateAssetCondition(asset1, false, regulation1)
                }),
                CreateAssetConditionLayer("3", 3, true, true, new List<IAssetCondition>
                {
                    CreateAssetCondition(asset1, true, null)
                }),
                CreateAssetConditionLayer("2", 2, true, true, new List<IAssetCondition>
                {
                    CreateAssetCondition(asset1, null, regulation2)
                })
            });

            // act
            IEnumerable<IAssetCondition> result = await _service.GetAssetConditionsByClient(clientId);

            IAssetCondition condition = result.First();

            // assert
            Assert.IsTrue(condition.Regulation == regulation2 && condition.AvailableToClient == true);
        }

        [TestMethod]
        public async Task GetAssetConditionsByClient_Use_Default_Settings_If_Conditions_Undefined()
        {
            // arrange
            const string clientId = "client1";
            const string assetId = "asset1";
            const string regulation = "r1";

            var defaultLayerSettings = new AssetConditionLayerSettings
            {
                SwiftDepositEnabled = true,
                ClientsCanCashInViaBankCards = true
            };

            var defaultAssetSettings = new AssetConditionSettings
            {
                Regulation = regulation,
                AvailableToClient = true
            };

            _layers.AddRange(new[]
            {
                CreateAssetConditionLayer("1", 1, true, true, new List<IAssetCondition>
                {
                    CreateAssetCondition(assetId, null, null)
                }),
            });

            _assetConditionLayerSettingsRepositoryMock.Setup(o => o.GetAsync())
                .Returns(Task.FromResult((IAssetConditionLayerSettings)defaultLayerSettings));

            _assetConditionSettingsRepositoryMock.Setup(o => o.GetAsync())
                .Returns(Task.FromResult((IAssetConditionSettings) defaultAssetSettings));

            // act
            IEnumerable<IAssetCondition> conditions = await _service.GetAssetConditionsByClient(clientId);

            IAssetCondition condition = conditions.First();

            // assert
            Assert.IsTrue(condition.Regulation == regulation && condition.AvailableToClient == true);
        }

        [TestMethod]
        public async Task GetAssetConditionsByClient_Override_Default_Settings()
        {
            // arrange
            const string clientId = "client1";
            const string assetId = "asset1";
            const string regulation1 = "r1";
            const string regulation2 = "r2";

            var defaultLayerSettings = new AssetConditionLayerSettings
            {
                SwiftDepositEnabled = true,
                ClientsCanCashInViaBankCards = true
            };

            var defaultAssetSettings = new AssetConditionSettings
            {
                Regulation = regulation1,
                AvailableToClient = true
            };

            _layers.AddRange(new[]
            {
                CreateAssetConditionLayer("1", 1, true, true, new List<IAssetCondition>
                {
                    CreateAssetCondition(assetId, false, regulation2)
                }),
            });

            _assetConditionLayerSettingsRepositoryMock.Setup(o => o.GetAsync())
                .Returns(Task.FromResult((IAssetConditionLayerSettings)defaultLayerSettings));

            _assetConditionSettingsRepositoryMock.Setup(o => o.GetAsync())
                .Returns(Task.FromResult((IAssetConditionSettings)defaultAssetSettings));

            // act
            IEnumerable<IAssetCondition> conditions = await _service.GetAssetConditionsByClient(clientId);

            IAssetCondition condition = conditions.First();

            // assert
            Assert.IsTrue(condition.Regulation == regulation2 && condition.AvailableToClient == false);
        }

        [TestMethod]
        public async Task GetAssetConditionsLayerSettingsByClient_Use_Default_Settings_If_No_Conditions()
        {
            // arrange
            const string clientId = "client1";

            var defaultLayerSettings = new AssetConditionLayerSettings
            {
                SwiftDepositEnabled = true,
                ClientsCanCashInViaBankCards = true
            };

            _assetConditionLayerSettingsRepositoryMock.Setup(o => o.GetAsync())
                .Returns(Task.FromResult((IAssetConditionLayerSettings)defaultLayerSettings));

            // act
            IAssetConditionLayerSettings settings = await _service.GetAssetConditionsLayerSettingsByClient(clientId);

            // assert
            Assert.AreEqual(defaultLayerSettings.ClientsCanCashInViaBankCards, settings.ClientsCanCashInViaBankCards);
            Assert.AreEqual(defaultLayerSettings.SwiftDepositEnabled, settings.SwiftDepositEnabled);
        }

        [TestMethod]
        public async Task GetAssetConditionsLayerSettingsByClient_Use_Default_Settings_If_Conditions_Undefined()
        {
            // arrange
            const string clientId = "client1";

            var defaultLayerSettings = new AssetConditionLayerSettings
            {
                SwiftDepositEnabled = true,
                ClientsCanCashInViaBankCards = true
            };

            _layers.AddRange(new[]
            {
                CreateAssetConditionLayer("1", 1, null, null, new List<IAssetCondition>())
            });

            _assetConditionLayerSettingsRepositoryMock.Setup(o => o.GetAsync())
                .Returns(Task.FromResult((IAssetConditionLayerSettings)defaultLayerSettings));

            // act
            IAssetConditionLayerSettings settings = await _service.GetAssetConditionsLayerSettingsByClient(clientId);

            // assert
            Assert.AreEqual(defaultLayerSettings.ClientsCanCashInViaBankCards, settings.ClientsCanCashInViaBankCards);
            Assert.AreEqual(defaultLayerSettings.SwiftDepositEnabled, settings.SwiftDepositEnabled);
        }

        [TestMethod]
        public async Task GetAssetConditionsLayerSettingsByClient_Override_Default_Settings()
        {
            // arrange
            const string clientId = "client1";

            _layers.AddRange(new[]
            {
                CreateAssetConditionLayer("1", 1, true, false, new List<IAssetCondition>())
            });
            
            // act
            IAssetConditionLayerSettings settings = await _service.GetAssetConditionsLayerSettingsByClient(clientId);

            // assert
            Assert.AreEqual(false, settings.ClientsCanCashInViaBankCards);
            Assert.AreEqual(true, settings.SwiftDepositEnabled);
        }

        private static AssetConditionLayerDto CreateAssetConditionLayer(string id, double priority, bool? swift, bool? cashIn, List<IAssetCondition> conditions)
        {
            return new AssetConditionLayerDto
            {
                Id = id,
                Description = null,
                Priority = (decimal)priority,
                SwiftDepositEnabled = swift,
                ClientsCanCashInViaBankCards = cashIn,
                AssetConditions = conditions
            };
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
