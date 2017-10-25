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
    public class AssetAttributesControllerTests
    {
        [TestMethod]
        public async Task Add__Correct_CreatedResult_Returned()
        {
            var serviceMock   = CreateServiceMock();
            var attributeMock = GenerateAssetAttribute();
            var assetId       = Guid.NewGuid().ToString();

            serviceMock
                .Setup(x => x.AddAsync(It.IsAny<string>(), It.IsAny<IAssetAttribute>()))
                .ReturnsAsync(() => attributeMock);
            

            var controller   = CreateController(serviceMock);
            var actionResult = await controller.Add(assetId, new AssetAttribute
            {
                Key   = attributeMock.Key,
                Value = attributeMock.Value
            });
            
            Assert.That.IsActionResultOfType<CreatedResult>(actionResult, out var createdResult);
            Assert.That.IsInstanceOfType<AssetAttribute>(createdResult.Value, out var attribute);
            Assert.That.AreEquivalent(attributeMock, attribute);
            
            var expectedLocation = $"api/v2/asset-attributes/{assetId}/{attribute.Key}";

            Assert.AreEqual(expectedLocation, createdResult.Location);
        }

        [TestMethod]
        public async Task Get__Asset_And_Key_Exists__Correct_OkResult_Returned()
        {
            var serviceMock   = CreateServiceMock();
            var assetId       = Guid.NewGuid().ToString();
            var attributeMock = GenerateAssetAttribute();

            serviceMock
                .Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(() => attributeMock);

            var controller   = CreateController(serviceMock);
            var actionResult = await controller.Get(assetId, attributeMock.Key);


            Assert.That.IsActionResultOfType<OkObjectResult>(actionResult, out var okResult);
            Assert.That.IsInstanceOfType<AssetAttribute>(okResult.Value, out var attribute);
            Assert.That.AreEquivalent(attributeMock, attribute);
        }

        [TestMethod]
        public async Task Get__Either_Asset_Or_Key_Not_Exists__NotFoundResult_Returned()
        {
            var serviceMock = CreateServiceMock();

            serviceMock
                .Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(() => null);

            var controller   = CreateController(serviceMock);
            var actionResult = await controller.Get(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task GetAll__Asset_Attributes_Exist__Correct_OkResult_Returned()
        {
            var serviceMock    = CreateServiceMock();
            var attributesMock = GenerateAssetAttributes();

            serviceMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(() => new [] { attributesMock });

            var controller   = CreateController(serviceMock);
            var actionResult = await controller.GetAll();

            Assert.That.IsActionResultOfType<OkObjectResult>(actionResult, out var okResult);
            Assert.That.IsInstanceOfType<IEnumerable<AssetAttributes>>(okResult.Value, out var attributes);
            Assert.AreEqual(1, attributes.Count());
            Assert.That.AreEquivalent(attributesMock, attributes.Single());
        }

        [TestMethod]
        public async Task GetAll__Asset_Attributes_Not_Exist__Correct_OkResult_Returned()
        {
            var serviceMock = CreateServiceMock();

            serviceMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(() => new List<IAssetAttributes>());

            var controller   = CreateController(serviceMock);
            var actionResult = await controller.GetAll();

            Assert.That.IsActionResultOfType<OkObjectResult>(actionResult, out var okResult);
            Assert.That.IsInstanceOfType<IEnumerable<AssetAttributes>>(okResult.Value, out var attributes);
            Assert.IsFalse(attributes.Any());
        }

        [TestMethod]
        public async Task GetAllForAsset__Asset_Exists__Correct_OkResult_Returned()
        {
            var serviceMock    = CreateServiceMock();
            var attributesMock = GenerateAssetAttributes();

            serviceMock
                .Setup(x => x.GetAllAsync(It.IsAny<string>()))
                .ReturnsAsync(() => new[] { attributesMock });

            var controller   = CreateController(serviceMock);
            var actionResult = await controller.GetAllForAsset(attributesMock.AssetId);

            Assert.That.IsActionResultOfType<OkObjectResult>(actionResult, out var okResult);
            Assert.That.IsInstanceOfType<AssetAttributes>(okResult.Value, out var attributes);
            Assert.That.AreEquivalent(attributesMock, attributes);
        }

        [TestMethod]
        public async Task GetAllForAsset__Asset_Not_Exists__NotFoundResult_Returned()
        {
            var serviceMock = CreateServiceMock();
            var assetId     = Guid.NewGuid().ToString();

            serviceMock
                .Setup(x => x.GetAllAsync(It.IsAny<string>()))
                .ReturnsAsync(() => new List<IAssetAttributes>());

            var controller   = CreateController(serviceMock);
            var actionResult = await controller.GetAllForAsset(assetId);

            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Remove__NoContentResult_Returned()
        {
            var serviceMock = CreateServiceMock();

            serviceMock
                .Setup(x => x.RemoveAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(false));

            var controller   = CreateController(serviceMock);
            var actionResult = await controller.Remove("", "");

            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult));

            serviceMock
                .Verify(x => x.RemoveAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task Update__NoContentResult_Returned()
        {
            var serviceMock = CreateServiceMock();

            serviceMock
                .Setup(x => x.UpdateAsync(It.IsAny<string>(), It.IsAny<AssetAttribute>()))
                .Returns(Task.FromResult(false));

            var controller   = CreateController(serviceMock);
            var actionResult = await controller.Update("", new AssetAttribute());

            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult));

            serviceMock
                .Verify(x => x.UpdateAsync(It.IsAny<string>(), It.IsAny<AssetAttribute>()), Times.Once);
        }

        private static AssetAttributesController CreateController(IMock<IAssetAttributeService> serviceMock)
        {
            return new AssetAttributesController(serviceMock.Object);
        }

        private static Mock<IAssetAttributeService> CreateServiceMock()
        {
            return new Mock<IAssetAttributeService>();
        }

        private static MockAssetAttributes GenerateAssetAttributes()
        {
            return new MockAssetAttributes
            {
                AssetId = Guid.NewGuid().ToString(),
                Attributes = new[]
                {
                    GenerateAssetAttribute()
                }
            };
        }

        private static MockAssetAttribute GenerateAssetAttribute()
        {
            return new MockAssetAttribute
            {
                Key   = Guid.NewGuid().ToString(),
                Value = Guid.NewGuid().ToString()
            };
        }


        public class MockAssetAttribute : IAssetAttribute
        {
            public string Key { get; set; }

            public string Value { get; set; }
        }

        public class MockAssetAttributes : IAssetAttributes
        {
            public string AssetId { get; set; }

            public IEnumerable<IAssetAttribute> Attributes { get; set; }
        }
    }

    public static class AssetAttributeAsserts
    {
        public static void AreEquivalent(this Assert assert, IAssetAttribute expected, AssetAttribute actual)
        {
            if (expected.Key   != actual.Key
            ||  expected.Value != actual.Value)
            {
                throw new AssertFailedException("Asset attributes do not match.");
            }
        }

        public static void AreEquivalent(this Assert assert, IAssetAttributes expected, AssetAttributes actual)
        {
            var actualAttributes   = actual.Attributes.ToArray();
            var expectedAttributes = expected.Attributes.ToArray();
        
            if (expected.AssetId          == actual.AssetId
            &&  expectedAttributes.Length == actualAttributes.Length)
            {
                for (var i = 0; i < expectedAttributes.Length; i++)
                {
                    assert.AreEquivalent(expectedAttributes[i], actualAttributes[i]);
                }
            }
            else
            {
                throw new AssertFailedException("Asset attributes do not match.");
            }
        }
    }
}