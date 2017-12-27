using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lykke.Service.Assets.Controllers.V2;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Responses.v2;
using Lykke.Service.Assets.Services.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Lykke.Service.Assets.Tests.v2
{
    [TestClass]
    public class AssetConditionsControllerTests
    {
        private readonly Mock<IAssetService> _assetServiceMock = new Mock<IAssetService>();
        private readonly Mock<IAssetConditionService> _assetConditionServiceMock = new Mock<IAssetConditionService>();
        private readonly Mock<IAssetConditionSettingsService> _assetConditionSettingsServiceMock = new Mock<IAssetConditionSettingsService>();

        private AssetConditionsController _controller;

        [TestInitialize]
        public void TestInitialized()
        {
            _controller = new AssetConditionsController(
                _assetServiceMock.Object,
                _assetConditionServiceMock.Object,
                _assetConditionSettingsServiceMock.Object);
        }

        [TestMethod]
        public async Task GetLayersAsync_Return_OK_Layers()
        {
            // arrange
            var layer = new AssetConditionLayer
            {
                Id = "1",
                Description = "description",
                Priority = 10,
                ClientsCanCashInViaBankCards = true,
                SwiftDepositEnabled = false
            };

            _assetConditionServiceMock.Setup(o => o.GetLayersAsync())
                .Returns(Task.FromResult((IEnumerable<IAssetConditionLayer>)new List<IAssetConditionLayer>
                {
                    layer
                }));

            // act
            IActionResult actionResult = await _controller.GetLayersAsync();

            var result = ((IEnumerable<AssetConditionLayerDto>) ((OkObjectResult) actionResult).Value).First();

            // assert
            Assert.IsTrue(AreEqual(result, layer));
        }

        [TestMethod]
        public async Task GetLayerByIdAsync_Return_OK_Layer()
        {
            // arrange
            const string layerId = "layer";

            var layer = new AssetConditionLayer
            {
                Id = "1",
                Description = "description",
                Priority = 10,
                ClientsCanCashInViaBankCards = true,
                SwiftDepositEnabled = false,
                AssetConditions = new List<IAssetCondition>
                {
                    new AssetCondition
                    {
                        Asset = "asset",
                        Regulation = "regulation",
                        AvailableToClient = true
                    }
                }
            };

            _assetConditionServiceMock.Setup(o => o.GetLayerAsync(It.IsAny<string>()))
                .Returns(Task.FromResult((IAssetConditionLayer) layer));

            // act
            IActionResult actionResult = await _controller.GetLayerByIdAsync(layerId);

            var result = (AssetConditionLayerDto) ((OkObjectResult) actionResult).Value;

            // assert
            Assert.IsTrue(AreEqual(result, layer));
        }

        private bool AreEqual(AssetConditionLayerDto a, IAssetConditionLayer b)
        {
            if (a == null && b == null)
                return true;

            if (a != null && b == null || a == null)
                return false;

            if (a.Id != b.Id || 
                a.Description != b.Description || 
                a.Priority != b.Priority ||
                a.ClientsCanCashInViaBankCards != b.ClientsCanCashInViaBankCards ||
                a.SwiftDepositEnabled != b.SwiftDepositEnabled)
                return false;

            if ((a.AssetConditions?.Count ?? 0) != (b.AssetConditions?.Count ?? 0))
                return false;

            return a.AssetConditions == null ||
                   a.AssetConditions.Count == 0 ||
                   a.AssetConditions.All(o => b.AssetConditions.Any(p => AreEqual(o, p)));
        }

        private bool AreEqual(AssetConditionDto a, IAssetCondition b)
        {
            if (a == null && b == null)
                return true;

            if (a != null && b == null || a == null)
                return false;

            return a.Asset == b.Asset && a.Regulation == b.Regulation && a.AvailableToClient == b.AvailableToClient;
        }
    }
}
