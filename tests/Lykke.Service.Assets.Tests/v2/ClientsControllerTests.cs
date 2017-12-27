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
    public class ClientsControllerTests
    {
        private readonly Mock<IAssetGroupService> _assetGroupServiceMock = new Mock<IAssetGroupService>();
        private readonly Mock<IAssetConditionService> _assetConditionServiceMock = new Mock<IAssetConditionService>();

        private ClientsController _controller;

        [TestInitialize]
        public void TestInitialized()
        {
            _controller = new ClientsController(_assetGroupServiceMock.Object, _assetConditionServiceMock.Object);
        }

        [TestMethod]
        public async Task GetAssetConditions_Returns_OK_Result()
        {
            // arrange
            const string clientId = "id";

            // act
            IActionResult result = await _controller.GetAssetConditions(clientId);

            // assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }


        [TestMethod]
        public async Task GetAssetConditions_Returns_Conditons()
        {
            // arrange
            const string clientId = "id";

            var condition = new AssetCondition
            {
                Asset = "asset",
                AvailableToClient = true,
                Regulation = "regulation"
            };

            _assetConditionServiceMock.Setup(o => o.GetAssetConditionsByClient(It.IsAny<string>()))
                .Returns(Task.FromResult((IEnumerable<IAssetCondition>) new List<IAssetCondition>
                {
                    condition
                }));

            // act
            IActionResult actionResult = await _controller.GetAssetConditions(clientId);

            var value = ((OkObjectResult)actionResult).Value as IEnumerable<AssetConditionDto>;

            // assert
            Assert.IsTrue(AreEqual(value?.FirstOrDefault(), condition));
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
