// Test temporaly disabled


//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Lykke.Service.Assets.Controllers.V2;
//using Lykke.Service.Assets.Core.Domain;
//using Lykke.Service.Assets.Core.Services;
//using Lykke.Service.Assets.Responses.V2;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Moq;

//namespace Lykke.Service.Assets.Tests.v2
//{
//    [TestClass]
//    public class AssetPairsControllerTests
//    {
//        [TestMethod]
//        public async Task Add__Correct_CreatedResult_Returned()
//        {
//            var serviceMock   = CreateServiceMock();
//            var assetPairMock = GenerateAssetPair();

//            serviceMock
//                .Setup(x => x.AddAsync(It.IsAny<IAssetPair>()))
//                .ReturnsAsync(() => assetPairMock);

//            var controller = CreateController(serviceMock);
//            var actionResult = await controller.Add(new AssetPair
//            {
//                Accuracy         = assetPairMock.Accuracy,
//                BaseAssetId      = assetPairMock.BaseAssetId,
//                Id               = assetPairMock.Id,
//                InvertedAccuracy = assetPairMock.InvertedAccuracy,
//                IsDisabled       = assetPairMock.IsDisabled,
//                Name             = assetPairMock.Name,
//                QuotingAssetId   = assetPairMock.QuotingAssetId,
//                Source           = assetPairMock.Source,
//                Source2          = assetPairMock.Source2
//            });

//            Assert.That.IsActionResultOfType<CreatedResult>(actionResult, out var createdResult);
//            Assert.That.IsInstanceOfType<AssetPair>(createdResult.Value, out var assetPair);
//            Assert.That.AreEquivalent(assetPairMock, assetPair);

//            var expectedLocation = $"api/v2/asset-pairs/{assetPair.Id}";

//            Assert.AreEqual(expectedLocation, createdResult.Location);
//        }

//        [TestMethod]
//        public async Task Get_Asset_Pair_Exists__Correct_OkResult_Returned()
//        {
//            var serviceMock   = CreateServiceMock();
//            var assetPairMock = GenerateAssetPair();

//            serviceMock
//                .Setup(x => x.GetAsync(It.IsAny<string>()))
//                .ReturnsAsync(() => assetPairMock);

//            var controller   = CreateController(serviceMock);
//            var actionResult = await controller.Get(Guid.NewGuid().ToString());

//            Assert.That.IsActionResultOfType<OkObjectResult>(actionResult, out var okResult);
//            Assert.That.IsInstanceOfType<AssetPair>(okResult.Value, out var assetPair);
//            Assert.That.AreEquivalent(assetPairMock, assetPair);
//        }

//        [TestMethod]
//        public async Task Get__Asset_Pair_Not_Exists__NotFoundResult_Returned()
//        {
//            var serviceMock = CreateServiceMock();

//            serviceMock
//                .Setup(x => x.GetAsync(It.IsAny<string>()))
//                .ReturnsAsync(() => null);

//            var controller = CreateController(serviceMock);
//            var actionResult = await controller.Get(Guid.NewGuid().ToString());

//            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
//        }

//        [TestMethod]
//        public async Task GetAll__Asset_Pairs_Exist__Correct_OkResult_Returned()
//        {
//            var serviceMock   = CreateServiceMock();
//            var assetPairMock = GenerateAssetPair();

//            serviceMock
//                .Setup(x => x.GetAllAsync())
//                .ReturnsAsync(() => new[] { assetPairMock });

//            var controller   = CreateController(serviceMock);
//            var actionResult = await controller.GetAll();

//            Assert.That.IsActionResultOfType<OkObjectResult>(actionResult, out var okResult);
//            Assert.That.IsInstanceOfType<IEnumerable<AssetPair>>(okResult.Value, out var assetPairs);
//            Assert.AreEqual(1, assetPairs.Count());
//            Assert.That.AreEquivalent(assetPairMock, assetPairs.Single());
//        }

//        [TestMethod]
//        public async Task GetAll__Asset_Pairs_Not_Exist__Correct_OkResult_Returned()
//        {
//            var serviceMock = CreateServiceMock();

//            serviceMock
//                .Setup(x => x.GetAllAsync())
//                .ReturnsAsync(() => new List<IAssetPair>());

//            var controller   = CreateController(serviceMock);
//            var actionResult = await controller.GetAll();

//            Assert.That.IsActionResultOfType<OkObjectResult>(actionResult, out var okResult);
//            Assert.That.IsInstanceOfType<IEnumerable<AssetPair>>(okResult.Value, out var assetPairs);
//            Assert.IsFalse(assetPairs.Any());
//        }

//        [TestMethod]
//        public async Task Remove__NoContentResult_Returned()
//        {
//            var serviceMock = CreateServiceMock();

//            serviceMock
//                .Setup(x => x.RemoveAsync(It.IsAny<string>()))
//                .Returns(Task.FromResult(false));

//            var controller = CreateController(serviceMock);
//            var actionResult = await controller.Remove("");

//            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult));

//            serviceMock
//                .Verify(x => x.RemoveAsync(It.IsAny<string>()), Times.Once);
//        }

//        [TestMethod]
//        public async Task Update__NoContentResult_Returned()
//        {
//            var serviceMock = CreateServiceMock();

//            serviceMock
//                .Setup(x => x.UpdateAsync(It.IsAny<AssetPair>()))
//                .Returns(Task.FromResult(false));

//            var controller = CreateController(serviceMock);
//            var actionResult = await controller.Update(new AssetPair());

//            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult));

//            serviceMock
//                .Verify(x => x.UpdateAsync(It.IsAny<AssetPair>()), Times.Once);
//        }


//        private static AssetPairsController CreateController(IMock<IAssetPairService> serviceMock)
//        {
//            return new AssetPairsController(serviceMock.Object);
//        }

//        private static Mock<IAssetPairService> CreateServiceMock()
//        {
//            return new Mock<IAssetPairService>();
//        }

//        public static MockAssetPair GenerateAssetPair()
//        {
//            return new MockAssetPair
//            {
//                Accuracy         = 42,
//                BaseAssetId      = Guid.NewGuid().ToString(),
//                Id               = Guid.NewGuid().ToString(),
//                InvertedAccuracy = 43,
//                IsDisabled       = true,
//                Name             = Guid.NewGuid().ToString(),
//                QuotingAssetId   = Guid.NewGuid().ToString(),
//                Source           = Guid.NewGuid().ToString(),
//                Source2          = Guid.NewGuid().ToString()
//            };
//        }


//        public class MockAssetPair : IAssetPair
//        {
//            public int Accuracy { get; set; }

//            public string BaseAssetId { get; set; }

//            public string Id { get; set; }

//            public int InvertedAccuracy { get; set; }

//            public bool IsDisabled { get; set; }

//            public string Name { get; set; }

//            public string QuotingAssetId { get; set; }

//            public string Source { get; set; }

//            public string Source2 { get; set; }
//        }
//    }

//    public static class AssetPairAsserts
//    {
//        public static void AreEquivalent(this Assert assert, IAssetPair expected, AssetPair actual)
//        {
//            if (expected.IsDisabled       != actual.IsDisabled
//             || expected.Accuracy         != actual.Accuracy
//             || expected.BaseAssetId      != actual.BaseAssetId
//             || expected.Id               != actual.Id
//             || expected.InvertedAccuracy != actual.InvertedAccuracy
//             || expected.Name             != actual.Name
//             || expected.QuotingAssetId   != actual.QuotingAssetId
//             || expected.Source           != actual.Source
//             || expected.Source2          != actual.Source2)
//            {
//                throw new AssertFailedException("Asset pairs do not match.");
//            }
//        }
//    }
//}
