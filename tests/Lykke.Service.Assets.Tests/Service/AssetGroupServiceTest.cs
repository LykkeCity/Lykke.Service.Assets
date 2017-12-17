using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Services.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Lykke.Service.Assets.Services.Tests
{
    [TestClass]
    public class AssetGroupServiceTest
    {
        private AssetGroupService _service;

        private readonly Mock<IClientAssetGroupLinkRepository> _clientAssetGroupLinkRepositoryMock = new Mock<IClientAssetGroupLinkRepository>();
        private readonly Mock<IAssetGroupClientLinkRepository> _assetGroupClientLinkRepositoryMock = new Mock<IAssetGroupClientLinkRepository>();
        private readonly Mock<IAssetGroupAssetLinkRepository> _assetGroupAssetLinkRepositoryMock = new Mock<IAssetGroupAssetLinkRepository>();
        private readonly Mock<IAssetGroupRepository> _assetGroupRepositoryMock = new Mock<IAssetGroupRepository>();
        private readonly Mock<IAssetConditionService> _assetConditionServiceMock = new Mock<IAssetConditionService>();
        private readonly Mock<IAssetsForClientCacheManager> _cacheManagerMock = new Mock<IAssetsForClientCacheManager>();

        [TestInitialize]
        public void TestInitialized()
        {
            _service = new AssetGroupService(
                _clientAssetGroupLinkRepositoryMock.Object, 
                _assetGroupClientLinkRepositoryMock.Object,
                _assetGroupAssetLinkRepositoryMock.Object,
                _cacheManagerMock.Object,
                _assetGroupRepositoryMock.Object, 
                _assetConditionServiceMock.Object);
        }

        [TestMethod]
        public async Task AssetConditionLayer_CashInViaBankCard_EnableGroup_EnableConditions_Then_Enabled()
        {
            var clientId = "123";

            SetWithiotCache();
            AddGroups(clientId, clientsCanCashInViaBankCards: true, swiftDepositEnabled: true);
            SetAssetConditionLayerSettings(clientId, clientsCanCashInViaBankCards: true, swiftDepositEnabled: true);

            var resAdr = await _service.CashInViaBankCardEnabledAsync(clientId, isIosDevice: false);
            var resIos = await _service.CashInViaBankCardEnabledAsync(clientId, isIosDevice: true);

            Assert.IsTrue(resAdr);
            Assert.IsTrue(resIos);
        }

        [TestMethod]
        public async Task AssetConditionLayer_CashInViaBankCard_Enabled_ButSwift_Disabled_Then_Enabled()
        {
            var clientId = "123";

            SetWithiotCache();
            AddGroups(clientId, clientsCanCashInViaBankCards: true, swiftDepositEnabled: false);
            SetAssetConditionLayerSettings(clientId, clientsCanCashInViaBankCards: true, swiftDepositEnabled: false);

            var resAdr = await _service.CashInViaBankCardEnabledAsync(clientId, isIosDevice: false);
            var resIos = await _service.CashInViaBankCardEnabledAsync(clientId, isIosDevice: true);

            Assert.IsTrue(resAdr);
            Assert.IsTrue(resIos);
        }


        [TestMethod]
        public async Task AssetConditionLayer_CashInViaBankCard_GroupDisabled_ConditionEnabled_Then_Disabled()
        {
            var clientId = "123";

            SetWithiotCache();
            AddGroups(clientId, clientsCanCashInViaBankCards: false, swiftDepositEnabled: true);
            SetAssetConditionLayerSettings(clientId, clientsCanCashInViaBankCards: true, swiftDepositEnabled: true);

            var resAdr = await _service.CashInViaBankCardEnabledAsync(clientId, isIosDevice: false);
            var resIos = await _service.CashInViaBankCardEnabledAsync(clientId, isIosDevice: true);

            Assert.IsFalse(resAdr);
            Assert.IsFalse(resIos);
        }

        [TestMethod]
        public async Task AssetConditionLayer_CashInViaBankCard_GroupEnabled_ConditionDisabled_Then_Disabled()
        {
            var clientId = "123";

            SetWithiotCache();
            AddGroups(clientId, clientsCanCashInViaBankCards: true, swiftDepositEnabled: true);
            SetAssetConditionLayerSettings(clientId, clientsCanCashInViaBankCards: false, swiftDepositEnabled: true);

            var resAdr = await _service.CashInViaBankCardEnabledAsync(clientId, isIosDevice: false);
            var resIos = await _service.CashInViaBankCardEnabledAsync(clientId, isIosDevice: true);

            Assert.IsFalse(resAdr);
            Assert.IsFalse(resIos);
        }



        [TestMethod]
        public async Task AssetConditionLayer_SwiftDepositEnabled_EnableGroup_EnableConditions_Then_Enabled()
        {
            var clientId = "123";

            SetWithiotCache();
            AddGroups(clientId, clientsCanCashInViaBankCards: true, swiftDepositEnabled: true);
            SetAssetConditionLayerSettings(clientId, clientsCanCashInViaBankCards: true, swiftDepositEnabled: true);

            var resAdr = await _service.SwiftDepositEnabledAsync(clientId, isIosDevice: false);
            var resIos = await _service.SwiftDepositEnabledAsync(clientId, isIosDevice: true);

            Assert.IsTrue(resAdr);
            Assert.IsTrue(resIos);
        }

        [TestMethod]
        public async Task AssetConditionLayer_SwiftDepositEnabled_Enabled_ButBankCard_Disabled_Then_Enabled()
        {
            var clientId = "123";

            SetWithiotCache();
            AddGroups(clientId, clientsCanCashInViaBankCards: false, swiftDepositEnabled: true);
            SetAssetConditionLayerSettings(clientId, clientsCanCashInViaBankCards: false, swiftDepositEnabled: true);

            var resAdr = await _service.SwiftDepositEnabledAsync(clientId, isIosDevice: false);
            var resIos = await _service.SwiftDepositEnabledAsync(clientId, isIosDevice: true);

            Assert.IsTrue(resAdr);
            Assert.IsTrue(resIos);
        }


        [TestMethod]
        public async Task AssetConditionLayer_SwiftDepositEnabled_GroupDisabled_ConditionEnabled_Then_Disabled()
        {
            var clientId = "123";

            SetWithiotCache();
            AddGroups(clientId, clientsCanCashInViaBankCards: true, swiftDepositEnabled: false);
            SetAssetConditionLayerSettings(clientId, clientsCanCashInViaBankCards: true, swiftDepositEnabled: true);

            var resAdr = await _service.SwiftDepositEnabledAsync(clientId, isIosDevice: false);
            var resIos = await _service.SwiftDepositEnabledAsync(clientId, isIosDevice: true);

            Assert.IsFalse(resAdr);
            Assert.IsFalse(resIos);
        }

        [TestMethod]
        public async Task AssetConditionLayer_SwiftDepositEnabled_GroupEnabled_ConditionDisabled_Then_Disabled()
        {
            var clientId = "123";

            SetWithiotCache();
            AddGroups(clientId, clientsCanCashInViaBankCards: true, swiftDepositEnabled: true);
            SetAssetConditionLayerSettings(clientId, clientsCanCashInViaBankCards: true, swiftDepositEnabled: false);

            var resAdr = await _service.SwiftDepositEnabledAsync(clientId, isIosDevice: false);
            var resIos = await _service.SwiftDepositEnabledAsync(clientId, isIosDevice: true);

            Assert.IsFalse(resAdr);
            Assert.IsFalse(resIos);
        }






        private void SetWithiotCache()
        {
            _cacheManagerMock.Setup(e => e.TryGetSaveCashInViaBankCardEnabledForClient(It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(Task.FromResult((bool?) null));
        }


        private void SetAssetConditionLayerSettings(string clientId, bool? clientsCanCashInViaBankCards, bool? swiftDepositEnabled)
        {
            var assetConditionLayerSettings = new AssetConditionLayerSettings()
            {
                ClientsCanCashInViaBankCards = clientsCanCashInViaBankCards,
                SwiftDepositEnabled = swiftDepositEnabled
            };

            _assetConditionServiceMock.Setup(e => e.GetAssetConditionsLayerSettingsByClient(clientId))
                .Returns(Task.FromResult<IAssetConditionLayerSettings>(assetConditionLayerSettings));
        }

        private void AddGroups(string clientId, bool clientsCanCashInViaBankCards, bool swiftDepositEnabled)
        {
            var groups = new List<IAssetGroupClientLink>()
            {
                new AssetGroupClientLink()
                {
                    ClientId = clientId,
                    GroupName = "1",
                    IsIosDevice = false,
                    ClientsCanCashInViaBankCards = clientsCanCashInViaBankCards,
                    SwiftDepositEnabled = swiftDepositEnabled
                },
                new AssetGroupClientLink()
                {
                    ClientId = clientId,
                    GroupName = "2",
                    IsIosDevice = true,
                    ClientsCanCashInViaBankCards = clientsCanCashInViaBankCards,
                    SwiftDepositEnabled = swiftDepositEnabled
                },
            };

            _assetGroupClientLinkRepositoryMock.Setup(e => e.GetAllAsync(clientId)).Returns(Task.FromResult<IEnumerable<IAssetGroupClientLink>>(groups));
        }
    }
}
