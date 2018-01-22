using System;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Services;
using Lykke.Service.Assets.Services.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

using Asset      = Lykke.Service.Assets.Services.Domain.Asset;
using Erc20Token = Lykke.Service.Assets.Services.Domain.Erc20Token;


namespace Lykke.Service.Assets.Tests.Service
{
    [TestClass]
    public class Erc20TokenAssetServiceTest
    {
        [DataTestMethod]
        [DataRow(0, "", "")]
        [DataRow(0, null, "")]
        [DataRow(0, "42000", "42000")]
        [DataRow(1, "42000", "4200")]
        [DataRow(2, "42000", "420")]
        [DataRow(3, "42000", "42")]
        [DataRow(4, "42000", "4.2")]
        [DataRow(5, "42000", "0.42")]
        [DataRow(6, "42000", "0.042")]
        public async Task CreateAssetForErc20TokenAsync__TokenTotalSupply_Converted_To_NumberOfTokens(
            int? tokenDecimals,
            string tokenTotalSupply,
            string expectedNumberOfTokens)
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
                cfg.AddProfile<Services.AutoMapperProfile>();
                cfg.AddProfile<Repositories.AutoMapperProfile>();
            });


            var assetServiceMock             = new Mock<IAssetService>();
            var assetExtendedInfoServiceMock = new Mock<IAssetExtendedInfoService>();
            var erc20TokenServiceMock        = new Mock<IErc20TokenService>();

            assetServiceMock
                .Setup(x => x.AddAsync(It.IsAny<IAsset>()))
                .ReturnsAsync(new Asset
                {
                    Id = $"{Guid.NewGuid():N}"
                });
            
            assetServiceMock
                .Setup(x => x.CreateDefault())
                .Returns(new Asset());

            assetExtendedInfoServiceMock
                .Setup(x => x.CreateDefault())
                .Returns(new AssetExtendedInfo());

            string actualNumberOfCoins = null;

            assetExtendedInfoServiceMock
                .Setup(x => x.AddAsync(It.IsAny<IAssetExtendedInfo>()))
                .Callback<IAssetExtendedInfo>(x =>
                {
                    actualNumberOfCoins = x.NumberOfCoins;
                })
                .ReturnsAsync(new AssetExtendedInfo());

            erc20TokenServiceMock
                .Setup(x => x.GetByTokenAddressAsync(It.IsAny<string>()))
                .ReturnsAsync(new Erc20Token
                {
                    TokenDecimals    = tokenDecimals,
                    TokenTotalSupply = tokenTotalSupply
                });


            var erc20TokenAssetService = new Erc20TokenAssetService
            (
                assetServiceMock.Object,
                assetExtendedInfoServiceMock.Object,
                erc20TokenServiceMock.Object
            );


            await erc20TokenAssetService.CreateAssetForErc20TokenAsync($"{Guid.NewGuid():N}");

            Assert.AreEqual(expectedNumberOfTokens, actualNumberOfCoins);
        }
    }
}
