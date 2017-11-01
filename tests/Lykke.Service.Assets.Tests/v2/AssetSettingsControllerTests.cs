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
    public class AssetSettingsControllerTests
    {
        [TestMethod]
        public async Task Add__Correct_CreatedResult_Returned()
        {
            var serviceMock = CreateServiceMock();
            var assetSettingsMock = GenerateAssetSettings();

            serviceMock
                .Setup(x => x.AddAsync(It.IsAny<IAssetSettings>()))
                .ReturnsAsync(() => assetSettingsMock);

            var controller   = CreateController(serviceMock);
            var actionResult = await controller.Add(new AssetSettings
            {
                Asset               = assetSettingsMock.Asset,
                CashinCoef          = assetSettingsMock.CashinCoef,
                ChangeWallet        = assetSettingsMock.ChangeWallet,
                Dust                = assetSettingsMock.Dust,
                HotWallet           = assetSettingsMock.HotWallet,
                MaxBalance          = assetSettingsMock.MaxBalance,
                MaxOutputsCount     = assetSettingsMock.MaxOutputsCount,
                MaxOutputsCountInTx = assetSettingsMock.MaxOutputsCountInTx,
                MinBalance          = assetSettingsMock.MinBalance,
                MinOutputsCount     = assetSettingsMock.MinOutputsCount,
                OutputSize          = assetSettingsMock.OutputSize,
                PrivateIncrement    = assetSettingsMock.PrivateIncrement
            });

            Assert.That.IsActionResultOfType<CreatedResult>(actionResult, out var createdResult);
            Assert.That.IsInstanceOfType<AssetSettings>(createdResult.Value, out var assetSettings);
            Assert.That.PropertiesAreEqual(assetSettingsMock, assetSettings);

            var expectedLocation = $"api/v2/asset-settings/{assetSettings.Asset}";

            Assert.AreEqual(expectedLocation, createdResult.Location);
        }

        [TestMethod]
        public async Task Get_Asset_Settings_Exist__Correct_OkResult_Returned()
        {
            var serviceMock       = CreateServiceMock();
            var assetSettingsMock = GenerateAssetSettings();

            serviceMock
                .Setup(x => x.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(() => assetSettingsMock);

            var controller   = CreateController(serviceMock);
            var actionResult = await controller.Get(Guid.NewGuid().ToString());

            Assert.That.IsActionResultOfType<OkObjectResult>(actionResult, out var okResult);
            Assert.That.IsInstanceOfType<AssetSettings>(okResult.Value, out var assetSettings);
            Assert.That.PropertiesAreEqual(assetSettingsMock, assetSettings);
        }

        [TestMethod]
        public async Task Get__Asset_Settings_Not_Exist__NotFoundResult_Returned()
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
        public async Task GetAll__Asset_Settings_Exist__Correct_OkResult_Returned()
        {
            var serviceMock       = CreateServiceMock();
            var assetSettingsMock = GenerateAssetSettings();

            serviceMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(() => new[] { assetSettingsMock });

            var controller   = CreateController(serviceMock);
            var actionResult = await controller.GetAll();

            Assert.That.IsActionResultOfType<OkObjectResult>(actionResult, out var okResult);
            Assert.That.IsInstanceOfType<ListOf<AssetSettings>>(okResult.Value, out var assetSettings);
            Assert.AreEqual(1, assetSettings.Items.Count());
            Assert.That.PropertiesAreEqual(assetSettingsMock, assetSettings.Items.Single());
        }

        [TestMethod]
        public async Task GetAll__Asset_Settings_Not_Exist__Correct_OkResult_Returned()
        {
            var serviceMock = CreateServiceMock();

            serviceMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(() => new List<IAssetSettings>());

            var controller = CreateController(serviceMock);
            var actionResult = await controller.GetAll();

            Assert.That.IsActionResultOfType<OkObjectResult>(actionResult, out var okResult);
            Assert.That.IsInstanceOfType<ListOf<AssetSettings>>(okResult.Value, out var assetSettings);
            Assert.IsFalse(assetSettings.Items.Any());
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
                .Setup(x => x.UpdateAsync(It.IsAny<AssetSettings>()))
                .Returns(Task.FromResult(false));

            var controller   = CreateController(serviceMock);
            var actionResult = await controller.Update(new AssetSettings());

            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult));

            serviceMock
                .Verify(x => x.UpdateAsync(It.IsAny<AssetSettings>()), Times.Once);
        }


        private static AssetSettingsController CreateController(IMock<IAssetSettingsService> serviceMock)
        {
            return new AssetSettingsController(serviceMock.Object);
        }

        private static Mock<IAssetSettingsService> CreateServiceMock()
        {
            return new Mock<IAssetSettingsService>();
        }

        public static MockAssetSettings GenerateAssetSettings()
        {
            return new MockAssetSettings
            {
                Asset               = Guid.NewGuid().ToString(),
                CashinCoef          = 0.42m,
                ChangeWallet        = Guid.NewGuid().ToString(),
                Dust                = 0.43m,
                HotWallet           = Guid.NewGuid().ToString(),
                MaxBalance          = 0.44m,
                MaxOutputsCount     = 42,
                MaxOutputsCountInTx = 43,
                MinBalance          = 0.45m,
                MinOutputsCount     = 44,
                OutputSize          = 0.46m,
                PrivateIncrement    = 45
            };
        }


        public class MockAssetSettings : IAssetSettings
        {
            public string Asset { get; set; }

            public decimal CashinCoef { get; set; }

            public string ChangeWallet { get; set; }

            public decimal Dust { get; set; }

            public string HotWallet { get; set; }

            public decimal? MaxBalance { get; set; }

            public int MaxOutputsCount { get; set; }

            public int MaxOutputsCountInTx { get; set; }

            public decimal MinBalance { get; set; }

            public int MinOutputsCount { get; set; }

            public decimal OutputSize { get; set; }

            public int PrivateIncrement { get; set; }
        }
    }

    public static class AssetSettingsAsserts
    {
        public static void PropertiesAreEqual(this Assert assert, IAssetSettings expected, AssetSettings actual)
        {
            Assert.AreEqual(expected.Asset,               actual.Asset);
            Assert.AreEqual(expected.CashinCoef,          actual.CashinCoef);
            Assert.AreEqual(expected.ChangeWallet,        actual.ChangeWallet);
            Assert.AreEqual(expected.Dust,                actual.Dust);
            Assert.AreEqual(expected.HotWallet,           actual.HotWallet);
            Assert.AreEqual(expected.MaxBalance,          actual.MaxBalance);
            Assert.AreEqual(expected.MaxOutputsCount,     actual.MaxOutputsCount);
            Assert.AreEqual(expected.MaxOutputsCountInTx, actual.MaxOutputsCountInTx);
            Assert.AreEqual(expected.MinBalance,          actual.MinBalance);
            Assert.AreEqual(expected.MinOutputsCount,     actual.MinOutputsCount);
            Assert.AreEqual(expected.OutputSize,          actual.OutputSize);
            Assert.AreEqual(expected.PrivateIncrement,    actual.PrivateIncrement);
        }
    }
}