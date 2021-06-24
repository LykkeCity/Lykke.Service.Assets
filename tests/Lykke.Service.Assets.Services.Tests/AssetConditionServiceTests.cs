using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Common.Log;
using Lykke.Common.Log;
using Lykke.Logs;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.NoSql.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Lykke.Service.Assets.Repositories.DTOs;
using Lykke.Service.Assets.Services.Domain;

namespace Lykke.Service.Assets.Services.Tests
{
    [TestClass]
    public class AssetConditionServiceTests
    {
        private readonly List<IAssetConditionLayer> _layers = new List<IAssetConditionLayer>();

        private readonly AssetDefaultConditionLayer _defaultConditionLayer = new AssetDefaultConditionLayer
        {
            Id = "DefaultLayer",
            AssetConditions = new List<IAssetCondition>()
        };

        private readonly Mock<IAssetConditionRepository> _assetConditionRepositoryMock =
            new Mock<IAssetConditionRepository>();

        private readonly Mock<IAssetDefaultConditionRepository> _assetDefaultConditionRepositoryMock =
            new Mock<IAssetDefaultConditionRepository>();

        private readonly Mock<IAssetDefaultConditionLayerRepository> _assetDefaultConditionLayerRepositoryMock =
            new Mock<IAssetDefaultConditionLayerRepository>();

        private readonly Mock<IAssetConditionLayerRepository> _assetConditionLayerRepositoryMock =
            new Mock<IAssetConditionLayerRepository>();

        private readonly Mock<IAssetConditionLayerLinkClientRepository> _assetConditionLayerLinkClientRepositoryMock =
            new Mock<IAssetConditionLayerLinkClientRepository>();

        private readonly Mock<IAssetsForClientCacheManager> _cacheMock =
            new Mock<IAssetsForClientCacheManager>();

        private readonly Mock<IDistributedCache<IAssetDefaultConditionLayer, AssetDefaultConditionLayerDto>>_assetDefaultConditionLayerCacheMock =
            new Mock<IDistributedCache<IAssetDefaultConditionLayer, AssetDefaultConditionLayerDto>>();

        private readonly Mock<IDistributedCache<IAssetCondition, AssetConditionDto>> _assetConditionCacheMock =
            new Mock<IDistributedCache<IAssetCondition, AssetConditionDto>>();

        private readonly Mock<IDistributedCache<IAssetDefaultCondition, AssetDefaultConditionDto>> _assetDefaultConditionCacheMock =
            new Mock<IDistributedCache<IAssetDefaultCondition, AssetDefaultConditionDto>>();

        private readonly Mock<IAssetsForClientCacheManager> _assetsForClientCacheManagerMock = new Mock<IAssetsForClientCacheManager>();

        private readonly Mock<IMyNoSqlWriterWrapper<AssetConditionNoSql>> _myNoSqlWriterMock = new Mock<IMyNoSqlWriterWrapper<AssetConditionNoSql>>();

        private AssetConditionService _service;

        [TestInitialize]
        public void TestInitialized()
        {
            var cachedAssetConditionsService = new CachedAssetConditionsService(
                _assetDefaultConditionLayerRepositoryMock.Object,
                _assetConditionRepositoryMock.Object,
                _assetDefaultConditionRepositoryMock.Object,
                _assetDefaultConditionLayerCacheMock.Object,
                _assetConditionCacheMock.Object,
                _assetDefaultConditionCacheMock.Object,
                _assetsForClientCacheManagerMock.Object
                );

            _service = new AssetConditionService(
                _assetConditionLayerRepositoryMock.Object,
                _assetDefaultConditionRepositoryMock.Object,
                _assetDefaultConditionLayerRepositoryMock.Object,
                _assetConditionLayerLinkClientRepositoryMock.Object,
                _cacheMock.Object,
                cachedAssetConditionsService,
                _myNoSqlWriterMock.Object,
                10,
                new HashSet<string>(),
                EmptyLogFactory.Instance
            );

            _assetConditionLayerLinkClientRepositoryMock.Setup(o => o.GetLayersAsync(It.IsAny<string>()))
                .Returns(Task.FromResult((IEnumerable<string>) new List<string>()));

            _assetConditionRepositoryMock.Setup(o => o.GetAsync(It.IsAny<string>()))
                .Returns(Task.FromResult((IEnumerable<IAssetCondition>) new List<IAssetCondition>()));

            _assetDefaultConditionRepositoryMock.Setup(o => o.GetAsync(It.IsAny<string>()))
                .Returns((string layerId) =>
                    Task.FromResult(_layers.FirstOrDefault(o => o.Id == layerId)?.AssetDefaultCondition));

            _assetDefaultConditionCacheMock.Setup(o => o.GetAsync(It.IsAny<string>(), It.IsAny<Func<Task<IAssetDefaultCondition>>>()))
                .Returns((string layerId, Func<Task<IAssetDefaultCondition>> fn) =>
                    Task.FromResult(Mapper.Map<AssetDefaultConditionDto>(_layers.FirstOrDefault(o => o.Id == layerId)?.AssetDefaultCondition)));

            _assetDefaultConditionLayerRepositoryMock.Setup(o => o.GetAsync())
                .Returns(Task.FromResult((IAssetDefaultConditionLayer)_defaultConditionLayer));

            _assetDefaultConditionLayerCacheMock.Setup(o => o.GetAsync(It.IsAny<string>(), It.IsAny<Func<Task<IAssetDefaultConditionLayer>>>()))
                .Returns(Task.FromResult(Mapper.Map<AssetDefaultConditionLayerDto>(_defaultConditionLayer)));

            _assetConditionLayerRepositoryMock.Setup(o => o.GetAsync(It.IsAny<IEnumerable<string>>()))
                .Returns(Task.FromResult((IEnumerable<IAssetConditionLayer>) _layers));

            _assetConditionRepositoryMock.Setup(o =>
                    o.GetAsync(It.Is<string>(p => p == _defaultConditionLayer.Id || _layers.Any(l => l.Id == p))))
                .Returns((string layerId) =>
                    Task.FromResult((IEnumerable<IAssetCondition>) (layerId == _defaultConditionLayer.Id
                        ? _defaultConditionLayer.AssetConditions
                        : _layers.First(o => o.Id == layerId).AssetConditions)));

            _assetConditionCacheMock.Setup(o =>
                    o.GetListAsync(It.Is<string>(p => p == _defaultConditionLayer.Id || _layers.Any(l => l.Id == p)),
                            It.IsAny<Func<Task<IEnumerable<IAssetCondition>>>>()))
                .Returns((string layerId, Func<Task<IEnumerable<IAssetCondition>>> fn) =>
                    Task.FromResult(Mapper.Map<IEnumerable<AssetConditionDto>>((layerId == _defaultConditionLayer.Id
                        ? _defaultConditionLayer.AssetConditions
                        : _layers.First(o => o.Id == layerId).AssetConditions))));
        }

        [TestMethod]
        public async Task GetLayerAsync_Return_Layer_With_Conditions()
        {
            // arrange
            var layer = new AssetConditionLayerDto
            {
                Id = "layer_1"
            };

            var conditions = new List<IAssetCondition>
            {
                new AssetCondition("asset_1"),
                new AssetCondition("asset_2")
            };

            var defaultConditions = new AssetDefaultCondition();

            _assetConditionLayerRepositoryMock.Setup(o => o.GetAsync(It.Is<string>(p => p == layer.Id)))
                .Returns(Task.FromResult((IAssetConditionLayer) layer));

            _assetConditionCacheMock.Setup(o => o.GetListAsync(It.Is<string>(p => p == layer.Id), It.IsAny<Func<Task<IEnumerable<IAssetCondition>>>>()))
                .Returns(Task.FromResult(Mapper.Map<IEnumerable<AssetConditionDto>>(conditions)));

            _assetDefaultConditionRepositoryMock.Setup(o => o.GetAsync(It.Is<string>(p => p == layer.Id)))
                .Returns(Task.FromResult((IAssetDefaultCondition) defaultConditions));

            // act
            IAssetConditionLayer result = await _service.GetLayerAsync(layer.Id);

            // assert
            Assert.IsTrue(result?.AssetConditions.Count == conditions.Count && result.AssetDefaultCondition != null);
        }

        [TestMethod]
        public async Task GetDefaultLayerAsync_Return_Layer_With_Conditions()
        {
            // arrange
            var layer = new AssetDefaultConditionLayer
            {
                Id = "DefaultLayer"
            };

            var conditions = new List<IAssetCondition>
            {
                new AssetCondition("asset_1"),
                new AssetCondition("asset_2")
            };

            _assetDefaultConditionLayerRepositoryMock.Setup(o => o.GetAsync())
                .Returns(Task.FromResult((IAssetDefaultConditionLayer) layer));

            _assetConditionCacheMock.Setup(o => o.GetListAsync(It.Is<string>(p => p == layer.Id), It.IsAny<Func<Task<IEnumerable<IAssetCondition>>>>()))
                .Returns(Task.FromResult(Mapper.Map<IEnumerable<AssetConditionDto>>(conditions)));

            // act
            IAssetDefaultConditionLayer result = await _service.GetDefaultLayerAsync();

            // assert
            Assert.AreEqual(conditions.Count, result?.AssetConditions.Count);
        }

        [TestMethod]
        public async Task DeleteLayerAsync_All_Related_Entities_Deleted()
        {
            // arrange
            string layerId = "layer_1";

            // act
            await _service.DeleteLayerAsync(layerId);

            // assert
            _assetConditionRepositoryMock.Verify(o => o.DeleteAsync(It.Is<string>(p => p == layerId)));
            _assetConditionLayerRepositoryMock.Verify(o => o.DeleteAsync(It.Is<string>(p => p == layerId)));
            _assetDefaultConditionRepositoryMock.Verify(o => o.DeleteAsync(It.Is<string>(p => p == layerId)));
            _assetConditionLayerLinkClientRepositoryMock.Verify(o => o.RemoveLayerFromClientsAsync(It.Is<string>(p => p == layerId)));
        }

        [TestMethod]
        public async Task GetClientLayers_Return_Layers_With_Conditions()
        {
            // arrange
            string clientId = "client_1";

            var layer = new AssetConditionLayerDto
            {
                Id = "layer_1"
            };

            var conditions = new List<IAssetCondition>
            {
                new AssetCondition("asset_1"),
                new AssetCondition("asset_2")
            };

            var defaultConditions = new AssetDefaultCondition();

            _assetConditionLayerLinkClientRepositoryMock.Setup(o => o.GetLayersAsync(It.Is<string>(p => p == clientId)))
                .Returns(Task.FromResult((IEnumerable<string>) new List<string>
                {
                    layer.Id
                }));

            _assetConditionLayerRepositoryMock.Setup(o =>
                    o.GetAsync(It.Is<IEnumerable<string>>(p => p.First() == layer.Id)))
                .Returns(Task.FromResult((IEnumerable<IAssetConditionLayer>) new List<IAssetConditionLayer>
                {
                    layer
                }));

            _assetConditionCacheMock.Setup(o => o.GetListAsync(It.Is<string>(p => p == layer.Id), It.IsAny<Func<Task<IEnumerable<IAssetCondition>>>>()))
                .Returns(Task.FromResult(Mapper.Map<IEnumerable<AssetConditionDto>>(conditions)));

            _assetDefaultConditionRepositoryMock.Setup(o => o.GetAsync(It.Is<string>(p => p == layer.Id)))
                .Returns(Task.FromResult((IAssetDefaultCondition)defaultConditions));

            // act
            IEnumerable<IAssetConditionLayer> result = await _service.GetClientLayers(clientId);

            // assert
            IAssetConditionLayer firstConditionLayer = result.First();
            Assert.IsTrue(firstConditionLayer.AssetConditions.Count == conditions.Count && firstConditionLayer.AssetDefaultCondition != null);
        }

        [TestMethod]
        public async Task GetAssetConditionsByClient_Valid_Merge_With_Default_Conditions()
        {
            // arrange
            const string clientId = "client_1";
            const string asset1 = "asset_1";
            const string asset2 = "asset_2";
            const string asset3 = "asset_3";
            const string regulation1 = "regulation_1";
            const string regulation2 = "regulation_2";

            _layers.AddRange(new[]
            {
                CreateAssetConditionLayer("1", 1, true, true, new List<IAssetCondition>
                {
                    CreateAssetCondition(asset1, false, regulation1),
                    CreateAssetCondition(asset2, false, regulation2)
                }),
                CreateAssetConditionLayer("3", 3, true, true, new List<IAssetCondition>
                {
                    CreateAssetCondition(asset1, true, null)
                }),
                CreateAssetConditionLayer("2", 2, true, true, new List<IAssetCondition>
                {
                    CreateAssetCondition(asset1, null, regulation2)
                })
            });

            ((List<IAssetCondition>)_defaultConditionLayer.AssetConditions).Add(CreateAssetCondition(asset3, true, regulation2));

            // act
            IEnumerable<IAssetCondition> result = await _service.GetAssetConditionsByClient(clientId);

            // assert
            Assert.AreEqual(3, result.Count());
        }

        [TestMethod]
        public async Task GetAssetConditionsByClient_No_Default_Layer_Conditions()
        {
            // arrange
            const string clientId = "client_1";
            const string asset1 = "asset_1";
            const string asset2 = "asset_2";
            const string regulation1 = "regulation_1";
            const string regulation2 = "regulation_2";

            _layers.AddRange(new[]
            {
                CreateAssetConditionLayer("1", 1, true, true, new List<IAssetCondition>
                {
                    CreateAssetCondition(asset1, false, regulation1),
                    CreateAssetCondition(asset2, false, regulation2)
                }),
                CreateAssetConditionLayer("3", 3, true, true, new List<IAssetCondition>
                {
                    CreateAssetCondition(asset1, true, null)
                }),
                CreateAssetConditionLayer("2", 2, true, true, new List<IAssetCondition>
                {
                    CreateAssetCondition(asset1, null, regulation2)
                })
            });

            // act
            IEnumerable<IAssetCondition> result = await _service.GetAssetConditionsByClient(clientId);

            // assert
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public async Task GetAssetConditionsByClient_No_Client_Layers_Use_Default_Layer_Conditions()
        {
            // arrange
            const string clientId = "client_1";
            const string asset1 = "asset_1";
            const string asset2 = "asset_2";
            const string asset3 = "asset_3";
            const string regulation1 = "regulation_1";
            const string regulation2 = "regulation_2";

            ((List<IAssetCondition>)_defaultConditionLayer.AssetConditions).Add(CreateAssetCondition(asset1, true, regulation2));
            ((List<IAssetCondition>)_defaultConditionLayer.AssetConditions).Add(CreateAssetCondition(asset2, true, regulation2));
            ((List<IAssetCondition>)_defaultConditionLayer.AssetConditions).Add(CreateAssetCondition(asset3, true, regulation1));

            // act
            IEnumerable<IAssetCondition> result = await _service.GetAssetConditionsByClient(clientId);

            // assert
            Assert.AreEqual(3, result.Count());
        }

        [TestMethod]
        public async Task GetAssetConditionsByClient_Top_Layer_Default_Condition_Overrides_Underlying_Conditions()
        {
            // arrange
            const string clientId = "client_1";
            const string asset1 = "asset_1";
            const string asset2 = "asset_2";
            const string asset3 = "asset_3";
            const string regulation1 = "regulation_1";
            const string regulation2 = "regulation_2";

            _layers.AddRange(new[]
            {
                CreateAssetConditionLayer("1", 1, true, true, new List<IAssetCondition>
                {
                    CreateAssetCondition(asset1, true, regulation2),
                    CreateAssetCondition(asset2, true, regulation2)
                }),
                CreateAssetConditionLayer("3", 3, true, true, new List<IAssetCondition>
                    {
                        CreateAssetCondition(asset1, true, regulation1)
                    },
                    new AssetDefaultCondition
                    {
                        Regulation = regulation1,
                        AvailableToClient = false
                    })
            });

            ((List<IAssetCondition>)_defaultConditionLayer.AssetConditions).Add(CreateAssetCondition(asset3, true, regulation2));

            // act
            IEnumerable<IAssetCondition> result = await _service.GetAssetConditionsByClient(clientId);

            // assert
            Assert.IsTrue(result.First(o => o.AvailableToClient == true).Asset == asset1);
        }


        [TestMethod]
        public async Task GetAssetConditionsByClient_Override_Default_Layer_Conditions()
        {
            // arrange
            const string clientId = "client_1";
            const string asset1 = "asset_1";
            const string asset2 = "asset_2";
            const string asset3 = "asset_3";
            const string regulation1 = "regulation_1";
            const string regulation2 = "regulation_2";

            _layers.AddRange(new[]
            {
                CreateAssetConditionLayer("1", 1, true, true, new List<IAssetCondition>
                {
                    CreateAssetCondition(asset1, false, regulation1),
                    CreateAssetCondition(asset2, false, regulation1),
                    CreateAssetCondition(asset3, false, regulation1)
                })
            });

            ((List<IAssetCondition>)_defaultConditionLayer.AssetConditions).Add(CreateAssetCondition(asset1, true, regulation2));
            ((List<IAssetCondition>)_defaultConditionLayer.AssetConditions).Add(CreateAssetCondition(asset2, true, regulation2));
            ((List<IAssetCondition>)_defaultConditionLayer.AssetConditions).Add(CreateAssetCondition(asset3, true, regulation2));

            // act
            IEnumerable<IAssetCondition> result = await _service.GetAssetConditionsByClient(clientId);

            // assert
            Assert.AreEqual(0, result.Count(o => o.AvailableToClient == true || o.Regulation == regulation2));
        }

        [TestMethod]
        public async Task GetAssetConditionsByClient_Override_Default_Layer_Conditions_Using_Default_Conditions()
        {
            // arrange
            const string clientId = "client_1";
            const string asset1 = "asset_1";
            const string asset2 = "asset_2";
            const string asset3 = "asset_3";
            const string regulation1 = "regulation_1";
            const string regulation2 = "regulation_2";

            _layers.AddRange(new[]
            {
                CreateAssetConditionLayer("1", 1, true, true, new List<IAssetCondition>(),
                new AssetDefaultCondition
                {
                    Regulation = regulation1,
                    AvailableToClient = false
                })
            });

            ((List<IAssetCondition>)_defaultConditionLayer.AssetConditions).Add(CreateAssetCondition(asset1, true, regulation2));
            ((List<IAssetCondition>)_defaultConditionLayer.AssetConditions).Add(CreateAssetCondition(asset2, true, regulation2));
            ((List<IAssetCondition>)_defaultConditionLayer.AssetConditions).Add(CreateAssetCondition(asset3, true, regulation2));

            // act
            IEnumerable<IAssetCondition> result = await _service.GetAssetConditionsByClient(clientId);

            // assert
            Assert.AreEqual(0, result.Count(o => o.AvailableToClient == true || o.Regulation == regulation2));
        }

        [TestMethod]
        public async Task GetAssetConditionsLayerSettingsByClient_Use_Default_Settings_If_No_Conditions()
        {
            // arrange
            const string clientId = "client1";

            _defaultConditionLayer.SwiftDepositEnabled = true;
            _defaultConditionLayer.ClientsCanCashInViaBankCards = true;

            // act
            IAssetConditionLayerSettings settings = await _service.GetAssetConditionsLayerSettingsByClient(clientId);

            // assert
            Assert.AreEqual(_defaultConditionLayer.ClientsCanCashInViaBankCards, settings.ClientsCanCashInViaBankCards);
            Assert.AreEqual(_defaultConditionLayer.SwiftDepositEnabled, settings.SwiftDepositEnabled);
        }

        [TestMethod]
        public async Task GetAssetConditionsLayerSettingsByClient_Use_Default_Settings_If_Conditions_Undefined()
        {
            // arrange
            const string clientId = "client1";

            _defaultConditionLayer.SwiftDepositEnabled = true;
            _defaultConditionLayer.ClientsCanCashInViaBankCards = true;

            _layers.AddRange(new[]
            {
                CreateAssetConditionLayer("1", 1, null, null, new List<IAssetCondition>())
            });

            // act
            IAssetConditionLayerSettings settings = await _service.GetAssetConditionsLayerSettingsByClient(clientId);

            // assert
            Assert.AreEqual(_defaultConditionLayer.ClientsCanCashInViaBankCards, settings.ClientsCanCashInViaBankCards);
            Assert.AreEqual(_defaultConditionLayer.SwiftDepositEnabled, settings.SwiftDepositEnabled);
        }

        [TestMethod]
        public async Task GetAssetConditionsLayerSettingsByClient_Override_Default_Settings()
        {
            // arrange
            const string clientId = "client1";

            _layers.AddRange(new[]
            {
                CreateAssetConditionLayer("1", 1, true, false, new List<IAssetCondition>())
            });

            // act
            IAssetConditionLayerSettings settings = await _service.GetAssetConditionsLayerSettingsByClient(clientId);

            // assert
            Assert.AreEqual(false, settings.ClientsCanCashInViaBankCards);
            Assert.AreEqual(true, settings.SwiftDepositEnabled);
        }

        private static AssetConditionLayerDto CreateAssetConditionLayer(string id, double priority, bool? swift, bool? cashIn, List<IAssetCondition> conditions, IAssetDefaultCondition defaultCondition = null)
        {
            return new AssetConditionLayerDto
            {
                Id = id,
                Description = null,
                Priority = (decimal)priority,
                SwiftDepositEnabled = swift,
                ClientsCanCashInViaBankCards = cashIn,
                AssetConditions = conditions,
                AssetDefaultCondition = defaultCondition,
            };
        }

        private static AssetCondition CreateAssetCondition(string asset, bool? availableToClient, string regulation)
        {
            return new AssetCondition(asset)
            {
                AvailableToClient = availableToClient,
                Regulation = regulation
            };
        }
    }
}
