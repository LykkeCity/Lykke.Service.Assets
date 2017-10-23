using System;
using System.Collections.Generic;
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
        public async Task Add__AssetId_And_Attribute_Passed__Correct_Created_Response_Returned()
        {
            var serviceMock = CreateServiceMock();

            var assetId        = Guid.NewGuid().ToString();
            var inputAttribute = new AssetAttribute
            {
                Key   = Guid.NewGuid().ToString(),
                Value = Guid.NewGuid().ToString()
            };
            

            serviceMock
                .Setup(x => x.AddAsync(It.IsAny<string>(), It.IsAny<IAssetAttribute>()))
                .ReturnsAsync(() => new MockAssetAttribute
                {
                    Key   = inputAttribute.Key,
                    Value = inputAttribute.Value
                });
            

            var controller   = CreateController(serviceMock);
            var actionResult = await controller.Add(assetId, inputAttribute);
            

            Assert.IsInstanceOfType(actionResult, typeof(CreatedResult));
            var createdResult = (CreatedResult) actionResult;


            Assert.IsInstanceOfType(createdResult.Value, typeof(IAssetAttribute));
            var outputAttribute = (IAssetAttribute) createdResult.Value;


            Assert.AreEqual(inputAttribute.Key,   outputAttribute.Key);
            Assert.AreEqual(inputAttribute.Value, outputAttribute.Value);


            var expectedLocation = $"api/v2/asset-attributes/{assetId}/{outputAttribute.Key}";
            Assert.AreEqual(expectedLocation, createdResult.Location);
        }

        [TestMethod]
        public async Task Get__Asset_And_Key_Exists__Correct_Ok_Response_Returned()
        {
            var serviceMock = CreateServiceMock();

            var assetId = Guid.NewGuid().ToString();
            var key     = Guid.NewGuid().ToString();
            var value   = Guid.NewGuid().ToString();

            serviceMock
                .Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(() => new MockAssetAttribute
                {
                    Key   = key,
                    Value = value
                });

            var controller   = CreateController(serviceMock);
            var actionResult = await controller.Get(assetId, key);

            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
            var okResult = (OkObjectResult)actionResult;

            Assert.IsInstanceOfType(okResult.Value, typeof(IAssetAttribute));
            var outputAttribute = (IAssetAttribute)okResult.Value;

            Assert.AreEqual(key,   outputAttribute.Key);
            Assert.AreEqual(value, outputAttribute.Value);
        }

        [TestMethod]
        public async Task Get__Either_Asset_Or_Key_Not_Exists__NotFound_Response_Returned()
        {
            var serviceMock = CreateServiceMock();

            serviceMock
                .Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(() => null);

            var controller   = CreateController(serviceMock);
            var actionResult = await controller.Get(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }


        private static AssetAttributesController CreateController(IMock<IAssetAttributeService> serviceMock)
        {
            return new AssetAttributesController(serviceMock.Object);
        }

        private static Mock<IAssetAttributeService> CreateServiceMock()
        {
            return new Mock<IAssetAttributeService>();
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
}