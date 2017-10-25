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
    public class AssetExtendedInfosControllerTests
    {
        [TestMethod]
        public async Task Add__Correct_CreatedResult_Returned()
        {
            var serviceMock      = CreateServiceMock();
            var extendedInfoMock = GenerateAssetExtendedInfo();

            serviceMock
                .Setup(x => x.AddAsync(It.IsAny<IAssetExtendedInfo>()))
                .ReturnsAsync(() => extendedInfoMock);

            var controller = CreateController(serviceMock);
            var actionResult = await controller.Add(new AssetExtendedInfo
            {
                AssetClass           = extendedInfoMock.AssetClass,
                AssetDescriptionUrl  = extendedInfoMock.AssetDescriptionUrl,
                Description          = extendedInfoMock.Description,
                FullName             = extendedInfoMock.FullName,
                Id                   = extendedInfoMock.Id,
                MarketCapitalization = extendedInfoMock.MarketCapitalization,
                NumberOfCoins        = extendedInfoMock.NumberOfCoins,
                PopIndex             = extendedInfoMock.PopIndex
            });

            Assert.That.IsActionResultOfType<CreatedResult>(actionResult, out var createdResult);
            Assert.That.IsInstanceOfType<AssetExtendedInfo>(createdResult.Value, out var extendedInfo);
            Assert.That.AreEquivalent(extendedInfoMock, extendedInfo);

            var expectedLocation = $"api/v2/asset-extended-infos/{extendedInfo.Id}";

            Assert.AreEqual(expectedLocation, createdResult.Location);
        }

        [TestMethod]
        public async Task Get_Asset_Extended_Info_Exists__Correct_OkResult_Returned()
        {
            var serviceMock      = CreateServiceMock();
            var extendedInfoMock = GenerateAssetExtendedInfo();

            serviceMock
                .Setup(x => x.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(() => extendedInfoMock);

            var controller   = CreateController(serviceMock);
            var actionResult = await controller.Get(Guid.NewGuid().ToString());

            Assert.That.IsActionResultOfType<OkObjectResult>(actionResult, out var okResult);
            Assert.That.IsInstanceOfType<AssetExtendedInfo>(okResult.Value, out var extendedInfo);
            Assert.That.AreEquivalent(extendedInfoMock, extendedInfo);
        }

        [TestMethod]
        public async Task Get__Asset_Extended_Info_Not_Exists__NotFoundResult_Returned()
        {
            var serviceMock = CreateServiceMock();

            serviceMock
                .Setup(x => x.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            var controller   = CreateController(serviceMock);
            var actionResult = await controller.Get(Guid.NewGuid().ToString());

            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task GetAll__Asset_Extended_Infos_Exist__Correct_OkResult_Returned()
        {
            var serviceMock      = CreateServiceMock();
            var extendedInfoMock = GenerateAssetExtendedInfo();

            serviceMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(() => new[] { extendedInfoMock });

            var controller = CreateController(serviceMock);
            var actionResult = await controller.GetAll();

            Assert.That.IsActionResultOfType<OkObjectResult>(actionResult, out var okResult);
            Assert.That.IsInstanceOfType<IEnumerable<AssetExtendedInfo>>(okResult.Value, out var extendedInfos);
            Assert.AreEqual(1, extendedInfos.Count());
            Assert.That.AreEquivalent(extendedInfoMock, extendedInfos.Single());
        }

        [TestMethod]
        public async Task GetAll__Asset_Extended_Infos_Not_Exist__Correct_OkResult_Returned()
        {
            var serviceMock = CreateServiceMock();

            serviceMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(() => new List<IAssetExtendedInfo>());

            var controller   = CreateController(serviceMock);
            var actionResult = await controller.GetAll();

            Assert.That.IsActionResultOfType<OkObjectResult>(actionResult, out var okResult);
            Assert.That.IsInstanceOfType<IEnumerable<AssetExtendedInfo>>(okResult.Value, out var assetExtendedInfos);
            Assert.IsFalse(assetExtendedInfos.Any());
        }

        [TestMethod]
        public async Task Remove__NoContentResult_Returned()
        {
            var serviceMock = CreateServiceMock();

            serviceMock
                .Setup(x => x.RemoveAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(false));

            var controller   = CreateController(serviceMock);
            var actionResult = await controller.Remove("");

            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult));

            serviceMock
                .Verify(x => x.RemoveAsync(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task Update__NoContentResult_Returned()
        {
            var serviceMock = CreateServiceMock();

            serviceMock
                .Setup(x => x.UpdateAsync(It.IsAny<AssetExtendedInfo>()))
                .Returns(Task.FromResult(false));

            var controller = CreateController(serviceMock);
            var actionResult = await controller.Update(new AssetExtendedInfo());

            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult));

            serviceMock
                .Verify(x => x.UpdateAsync(It.IsAny<AssetExtendedInfo>()), Times.Once);
        }


        private static AssetExtendedInfosController CreateController(IMock<IAssetExtendedInfoService> serviceMock)
        {
            return new AssetExtendedInfosController(serviceMock.Object);
        }

        private static Mock<IAssetExtendedInfoService> CreateServiceMock()
        {
            return new Mock<IAssetExtendedInfoService>();
        }

        private static MockAssetExtendedInfo GenerateAssetExtendedInfo()
        {
            return new MockAssetExtendedInfo
            {
                AssetClass           = "Asset class",
                AssetDescriptionUrl  = "http://asset.description.url",
                Description          = "Asset description",
                FullName             = "Asset full name",
                Id                   = Guid.NewGuid().ToString(),
                MarketCapitalization = "1000",
                NumberOfCoins        = "2000",
                PopIndex             = 42
            };
        }


        public class MockAssetExtendedInfo : IAssetExtendedInfo
        {
            public string AssetClass { get; set; }

            public string AssetDescriptionUrl { get; set; }

            public string Description { get; set; }

            public string FullName { get; set; }

            public string Id { get; set; }

            public string MarketCapitalization { get; set; }

            public string NumberOfCoins { get; set; }

            public int PopIndex { get; set; }
        }
    }

    public static class AssetExtendedInfoAsserts
    {
        public static void AreEquivalent(this Assert assert, IAssetExtendedInfo expected, AssetExtendedInfo actual)
        {
            if (expected.AssetClass           != actual.AssetClass
             || expected.AssetDescriptionUrl  != actual.AssetDescriptionUrl
             || expected.Description          != actual.Description
             || expected.FullName             != actual.FullName
             || expected.Id                   != actual.Id
             || expected.MarketCapitalization != actual.MarketCapitalization
             || expected.NumberOfCoins        != actual.NumberOfCoins
             || expected.PopIndex             != actual.PopIndex)
            {
                throw new AssertFailedException("Asset extended infos do not match.");
            }
        }
    }
}