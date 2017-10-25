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
    public class AssetCategoriesControllerTests
    {
        [TestMethod]
        public async Task Add()
        {
            var serviceMock  = CreateServiceMock();
            var categoryMock = GenerateAssetCategory();

            serviceMock
                .Setup(x => x.AddAsync(It.IsAny<IAssetCategory>()))
                .ReturnsAsync(() => categoryMock);

            var controller   = CreateController(serviceMock);
            var actionResult = await controller.Add(new AssetCategory
            {
                AndroidIconUrl = categoryMock.AndroidIconUrl,
                Id             = categoryMock.Id,
                IosIconUrl     = categoryMock.IosIconUrl,
                Name           = categoryMock.Name,
                SortOrder      = categoryMock.SortOrder
            });

            Assert.That.IsActionResultOfType<CreatedResult>(actionResult, out var createdResult);
            Assert.That.IsInstanceOfType<AssetCategory>(createdResult.Value, out var category);
            Assert.That.AreEquivalent(categoryMock, category);

            var expectedLocation = $"api/v2/asset-categories/{category.Id}";

            Assert.AreEqual(expectedLocation, createdResult.Location);
        }

        [TestMethod]
        public async Task Get_Asset_Category_Exists__Correct_OkResult_Returned()
        {
            var serviceMock  = CreateServiceMock();
            var categoryMock = GenerateAssetCategory();

            serviceMock
                .Setup(x => x.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(() => categoryMock);

            var controller   = CreateController(serviceMock);
            var actionResult = await controller.Get(Guid.NewGuid().ToString());

            Assert.That.IsActionResultOfType<OkObjectResult>(actionResult, out var okResult);
            Assert.That.IsInstanceOfType<AssetCategory>(okResult.Value, out var category);
            Assert.That.AreEquivalent(categoryMock, category);
        }

        [TestMethod]
        public async Task Get__Asset_Category_Not_Exists__NotFoundResult_Returned()
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
        public async Task GetAll__Asset_Categories_Exist__Correct_OkResult_Returned()
        {
            var serviceMock  = CreateServiceMock();
            var categoryMock = GenerateAssetCategory();

            serviceMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(() => new [] { categoryMock });

            var controller   = CreateController(serviceMock);
            var actionResult = await controller.GetAll();

            Assert.That.IsActionResultOfType<OkObjectResult>(actionResult, out var okResult);
            Assert.That.IsInstanceOfType<IEnumerable<AssetCategory>>(okResult.Value, out var categories);
            Assert.AreEqual(1, categories.Count());
            Assert.That.AreEquivalent(categoryMock, categories.Single());
        }

        [TestMethod]
        public async Task GetAll__Asset_Categories_Not_Exist__Correct_OkResult_Returned()
        {
            var serviceMock = CreateServiceMock();

            serviceMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(() => new List<IAssetCategory>());

            var controller   = CreateController(serviceMock);
            var actionResult = await controller.GetAll();

            Assert.That.IsActionResultOfType<OkObjectResult>(actionResult, out var okResult);
            Assert.That.IsInstanceOfType<IEnumerable<AssetCategory>>(okResult.Value, out var attributes);
            Assert.IsFalse(attributes.Any());
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
                .Setup(x => x.UpdateAsync(It.IsAny<AssetCategory>()))
                .Returns(Task.FromResult(false));

            var controller   = CreateController(serviceMock);
            var actionResult = await controller.Update(new AssetCategory());

            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult));

            serviceMock
                .Verify(x => x.UpdateAsync(It.IsAny<AssetCategory>()), Times.Once);
        }


        private static AssetCategoriesController CreateController(IMock<IAssetCategoryService> serviceMock)
        {
            return new AssetCategoriesController(serviceMock.Object);
        }

        private static Mock<IAssetCategoryService> CreateServiceMock()
        {
            return new Mock<IAssetCategoryService>();
        }

        public static MockAssetCategory GenerateAssetCategory()
        {
            return new MockAssetCategory
            {
                AndroidIconUrl = "http://android.icon.url",
                Id             = Guid.NewGuid().ToString(),
                IosIconUrl     = "http://ios.icon.url",
                Name           = Guid.NewGuid().ToString(),
                SortOrder      = 42
            };
        }


        public class MockAssetCategory : IAssetCategory
        {
            public string AndroidIconUrl { get; set; }

            public string Id { get; set; }

            public string IosIconUrl { get; set; }

            public string Name { get; set; }

            public int SortOrder { get; set; }
        }
    }

    public static class AssetCategoryAsserts
    {
        public static void AreEquivalent(this Assert assert, IAssetCategory expected, AssetCategory actual)
        {
            if (expected.AndroidIconUrl != actual.AndroidIconUrl
             || expected.Id             != actual.Id
             || expected.IosIconUrl     != actual.IosIconUrl
             || expected.Name           != actual.Name
             || expected.SortOrder      != actual.SortOrder)
            {
                throw new AssertFailedException("Asset categories do not match.");
            }
        }
    }
}