using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lykke.Service.Assets.Controllers.V2;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Responses.V2;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Lykke.Service.Assets.Tests.v2
{
    [TestClass]
    public class AssetGroupsControllerTests
    {
        [TestMethod]
        public async Task Add__Correct_CreatedResult_Returned()
        {
            var serviceMock = CreateServiceMock();
            var groupMock   = GenerateAssetGroup();

            serviceMock
                .Setup(x => x.AddGroupAsync(It.IsAny<IAssetGroup>()))
                .ReturnsAsync(() => groupMock);

            var controller = CreateController(serviceMock);
            var actionResult = await controller.Add(new AssetGroup
            {
                ClientsCanCashInViaBankCards = groupMock.ClientsCanCashInViaBankCards,
                IsIosDevice                  = groupMock.IsIosDevice,
                Name                         = groupMock.Name,
                SwiftDepositEnabled          = groupMock.SwiftDepositEnabled
            });

            Assert.That.IsActionResultOfType<CreatedResult>(actionResult, out var createdResult);
            Assert.That.IsInstanceOfType<AssetGroup>(createdResult.Value, out var group);
            Assert.That.AreEquivalent(groupMock, group);

            var expectedLocation = $"api/v2/asset-groups/{group.Name}";

            Assert.AreEqual(expectedLocation, createdResult.Location);
        }

        [TestMethod]
        public async Task AddAsset__NoContentResult_Returned()
        {
            var serviceMock = CreateServiceMock();

            serviceMock
                .Setup(x => x.AddAssetToGroupAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(false));

            var controller   = CreateController(serviceMock);
            var actionResult = await controller.AddAsset("", "");

            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult));

            serviceMock
                .Verify(x => x.AddAssetToGroupAsync(It.IsAny<string>(), It.IsAny<string>()));
        }

        [TestMethod]
        public async Task AddClient__NoContentResult_Returned()
        {
            var serviceMock = CreateServiceMock();

            serviceMock
                .Setup(x => x.AddClientToGroupAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(false));

            var controller   = CreateController(serviceMock);
            var actionResult = await controller.AddClient("", "");

            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult));

            serviceMock
                .Verify(x => x.AddClientToGroupAsync(It.IsAny<string>(), It.IsAny<string>()));
        }

        [TestMethod]
        public async Task Get__Asset_Group_Exists__Correct_OkResult_Returned()
        {
            var serviceMock = CreateServiceMock();
            var groupMock   = GenerateAssetGroup();

            serviceMock
                .Setup(x => x.GetGroupAsync(It.IsAny<string>()))
                .ReturnsAsync(() => groupMock);

            var controller   = CreateController(serviceMock);
            var actionResult = await controller.Get("");

            Assert.That.IsActionResultOfType<OkObjectResult>(actionResult, out var okResult);
            Assert.That.IsInstanceOfType<AssetGroup>(okResult.Value, out var group);
            Assert.That.AreEquivalent(groupMock, group);
        }

        [TestMethod]
        public async Task Get__Asset_Group_Not_Exists__NotFoundResult_Returned()
        {
            var serviceMock = CreateServiceMock();
            
            serviceMock
                .Setup(x => x.GetGroupAsync(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            var controller   = CreateController(serviceMock);
            var actionResult = await controller.Get("");

            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task GetAll__AssetGroups_Exist__Correct_OkResult__Returned()
        {
            var serviceMock = CreateServiceMock();
            var groupMock   = GenerateAssetGroup();

            serviceMock
                .Setup(x => x.GetAllGroupsAsync())
                .ReturnsAsync(() => new[] { groupMock });

            var controller   = CreateController(serviceMock);
            var actionResult = await controller.GetAll();

            Assert.That.IsActionResultOfType<OkObjectResult>(actionResult, out var okResult);
            Assert.That.IsInstanceOfType<IEnumerable<AssetGroup>>(okResult.Value, out var groups);
            Assert.AreEqual(1, groups.Count());
            Assert.That.AreEquivalent(groupMock, groups.Single());
        }

        [TestMethod]
        public async Task GetAll__AssetGroups_Not_Exist__Correct_OkResult__Returned()
        {
            var serviceMock = CreateServiceMock();

            serviceMock
                .Setup(x => x.GetAllGroupsAsync())
                .ReturnsAsync(() => new List<IAssetGroup>());

            var controller = CreateController(serviceMock);
            var actionResult = await controller.GetAll();

            Assert.That.IsActionResultOfType<OkObjectResult>(actionResult, out var okResult);
            Assert.That.IsInstanceOfType<IEnumerable<AssetGroup>>(okResult.Value, out var groups);
            Assert.IsFalse(groups.Any());
        }

        [TestMethod]
        public async Task GetAssetIds__Group_Exists_And_Contains_Asset_Ids__Correct_OkResult_Returned()
        {
            var serviceMock = CreateServiceMock();
            var assetIdMock = Guid.NewGuid().ToString();

            serviceMock
                .Setup(x => x.GetAssetIdsForGroupAsync(It.IsAny<string>()))
                .ReturnsAsync(() => new [] { assetIdMock });

            var controller   = CreateController(serviceMock);
            var actionResult = await controller.GetAssetIds("");

            Assert.That.IsActionResultOfType<OkObjectResult>(actionResult, out var okResult);
            Assert.That.IsInstanceOfType<IEnumerable<string>>(okResult.Value, out var assetIds);
            Assert.AreEqual(1, assetIds.Count());
            Assert.AreEqual(assetIdMock, assetIds.Single());
        }

        [TestMethod]
        public async Task GetAssetIds__Group_Not_Exists_Or_Not_Contains_Asset_Ids__Correct_OkResult_Returned()
        {
            var serviceMock = CreateServiceMock();

            serviceMock
                .Setup(x => x.GetAssetIdsForGroupAsync(It.IsAny<string>()))
                .ReturnsAsync(() => new List<string>());

            var controller = CreateController(serviceMock);
            var actionResult = await controller.GetAssetIds("");

            Assert.That.IsActionResultOfType<OkObjectResult>(actionResult, out var okResult);
            Assert.That.IsInstanceOfType<IEnumerable<string>>(okResult.Value, out var assetIds);
            Assert.IsFalse(assetIds.Any());
        }

        [TestMethod]
        public async Task GetClientIds__Group_Exists_And_Contains_Client_Ids__Correct_OkResult_Returned()
        {
            var serviceMock  = CreateServiceMock();
            var clientIdMock = Guid.NewGuid().ToString();

            serviceMock
                .Setup(x => x.GetClientIdsForGroupAsync(It.IsAny<string>()))
                .ReturnsAsync(() => new [] { clientIdMock });

            var controller   = CreateController(serviceMock);
            var actionResult = await controller.GetClientIds("");

            Assert.That.IsActionResultOfType<OkObjectResult>(actionResult, out var okResult);
            Assert.That.IsInstanceOfType<IEnumerable<string>>(okResult.Value, out var clientIds);
            Assert.AreEqual(1, clientIds.Count());
            Assert.AreEqual(clientIdMock, clientIds.Single());
        }

        [TestMethod]
        public async Task GetClientIds__Group_Not_Exists_Or_Not_Contains_Client_Ids__Correct_OkResult_Returned()
        {
            var serviceMock = CreateServiceMock();

            serviceMock
                .Setup(x => x.GetClientIdsForGroupAsync(It.IsAny<string>()))
                .ReturnsAsync(() => new List<string>());

            var controller   = CreateController(serviceMock);
            var actionResult = await controller.GetClientIds("");

            Assert.That.IsActionResultOfType<OkObjectResult>(actionResult, out var okResult);
            Assert.That.IsInstanceOfType<IEnumerable<string>>(okResult.Value, out var clientIds);
            Assert.IsFalse(clientIds.Any());
        }

        [TestMethod]
        public async Task Remove__NoContent_Result_Returned()
        {
            var serviceMock = CreateServiceMock();

            serviceMock
                .Setup(x => x.RemoveGroupAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(false));

            var controller   = CreateController(serviceMock);
            var actionResult = await controller.Remove("");

            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult));

            serviceMock
                .Verify(x => x.RemoveGroupAsync(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task RemoveAsset__NoContent_Result_Returned()
        {
            var serviceMock = CreateServiceMock();

            serviceMock
                .Setup(x => x.RemoveAssetFromGroupAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(false));

            var controller   = CreateController(serviceMock);
            var actionResult = await controller.RemoveAsset("", "");

            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult));

            serviceMock
                .Verify(x => x.RemoveAssetFromGroupAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task RemoveClient__NoContent_Result_Returned()
        {
            var serviceMock = CreateServiceMock();

            serviceMock
                .Setup(x => x.RemoveClientFromGroupAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(false));

            var controller   = CreateController(serviceMock);
            var actionResult = await controller.RemoveClient("", "");

            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult));

            serviceMock
                .Verify(x => x.RemoveClientFromGroupAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task Update__NoContent_Result_Returned()
        {
            var serviceMock = CreateServiceMock();

            serviceMock
                .Setup(x => x.UpdateGroupAsync(It.IsAny<AssetGroup>()))
                .Returns(Task.FromResult(false));

            var controller   = CreateController(serviceMock);
            var actionResult = await controller.Update(new AssetGroup());

            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult));

            serviceMock
                .Verify(x => x.UpdateGroupAsync(It.IsAny<AssetGroup>()), Times.Once);
        }


        private static AssetGroupsController CreateController(IMock<IAssetGroupService> serviceMock)
        {
            return new AssetGroupsController(serviceMock.Object);
        }

        private static Mock<IAssetGroupService> CreateServiceMock()
        {
            return new Mock<IAssetGroupService>();
        }

        private static MockAssetGroup GenerateAssetGroup()
        {
            return new MockAssetGroup
            {
                ClientsCanCashInViaBankCards = true,
                IsIosDevice                  = true,
                Name                         = Guid.NewGuid().ToString(),
                SwiftDepositEnabled          = true
            };
        }


        public class MockAssetGroup : IAssetGroup
        {
            public bool ClientsCanCashInViaBankCards { get; set; }

            public bool IsIosDevice { get; set; }

            public string Name { get; set; }

            public bool SwiftDepositEnabled { get; set; }
        }
    }

    public static class AsseGroupAsserts
    {
        public static void AreEquivalent(this Assert assert, IAssetGroup expected, AssetGroup actual)
        {
            if (expected.ClientsCanCashInViaBankCards != actual.ClientsCanCashInViaBankCards
             || expected.IsIosDevice                  != actual.IsIosDevice
             || expected.Name                         != actual.Name
             || expected.SwiftDepositEnabled          != actual.SwiftDepositEnabled)
            {
                throw new AssertFailedException("Asset extended infos do not match.");
            }
        }
    }
}