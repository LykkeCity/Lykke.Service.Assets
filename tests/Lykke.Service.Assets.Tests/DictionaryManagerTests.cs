using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Lykke.Service.Assets.Tests
{
    [TestClass]
    public class DictionaryManagerTests
    {
        [UsedImplicitly]
        public class TestItem : IDictionaryItem
        {
            public string Id { get; set; }
        }

        private readonly TimeSpan _cacheExpirationPeriod = TimeSpan.FromMinutes(1);

        private IDictionaryManager<TestItem> _manager;
        private Mock<IDictionaryRepository<TestItem>> _repositoryMock;
        private Mock<IDictionaryCacheService<TestItem>> _cacheServiceMock;
        private Mock<IDateTimeProvider> _dateTimeProviderMock;

        [TestInitialize]
        public void InitializeTest()
        {
            _repositoryMock = new Mock<IDictionaryRepository<TestItem>>();
            _cacheServiceMock = new Mock<IDictionaryCacheService<TestItem>>();
            _dateTimeProviderMock = new Mock<IDateTimeProvider>();

            _manager = new DictionaryManager<TestItem>(_repositoryMock.Object, _cacheServiceMock.Object, _dateTimeProviderMock.Object, _cacheExpirationPeriod);
        }

        #region Getting item

        [TestMethod]
        public async Task Getting_enabled_pair_returns_existing_item()
        {
            // Arrange
            _cacheServiceMock
                .Setup(s => s.TryGet(It.Is<string>(a => a == "EURUSD")))
                .Returns((string a) => new TestItem{ Id = a });

            // Act
            var item = await _manager.TryGetAsync("EURUSD");

            // Assert
            Assert.IsNotNull(item);
            Assert.AreEqual("EURUSD", item.Id);
        }

        [TestMethod]
        public async Task Getting_enabled_pair_not_returns_missing_item()
        {
            // Arrange
            _cacheServiceMock
                .Setup(s => s.TryGet(It.Is<string>(a => a == "EURUSD")))
                .Returns((string a) => null);

            // Act
            var item = await _manager.TryGetAsync("EURUSD");

            // Assert
            Assert.IsNull(item);
        }

        #endregion


        #region Getting all items

        [TestMethod]
        public async Task Getting_all_items_returns_empty_enumerable_if_no_items()
        {
            // Arrange
            _cacheServiceMock
                .Setup(s => s.GetAll())
                .Returns(() => new TestItem[0]);

            // Act
            var items = await _manager.GetAllAsync();

            // Assert
            Assert.IsNotNull(items);
            Assert.IsFalse(items.Any());
        }

        [TestMethod]
        public async Task Getting_all_items_returns_items()
        {
            // Arrange
            _cacheServiceMock
                .Setup(s => s.GetAll())
                .Returns(() => new[]
                {
                    new TestItem { Id = "EURUSD" },
                    new TestItem { Id = "USDRUB" },
                    new TestItem { Id = "USDCHF" }
                });

            // Act
            var pairs = (await _manager.GetAllAsync()).ToArray();

            // Assert
            Assert.AreEqual(3, pairs.Length);
            Assert.AreEqual("EURUSD", pairs[0].Id);
            Assert.AreEqual("USDRUB", pairs[1].Id);
            Assert.AreEqual("USDCHF", pairs[2].Id);
        }

        #endregion


        #region Cache updating

        [TestMethod]
        public async Task Getting_item_first_time_updates_cache()
        {
            // Arrange
            _dateTimeProviderMock.SetupGet(p => p.UtcNow).Returns(() => new DateTime(2017, 06, 23, 21, 00, 00));
            _repositoryMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(() => new[]
                {
                    new TestItem {Id = "EURUSD"},
                });

            // Act
            await _manager.TryGetAsync("EURUSD");

            // Asert
            _repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
            // ReSharper disable once PossibleMultipleEnumeration
            _cacheServiceMock.Verify(s => s.Update(It.Is<IEnumerable<TestItem>>(p => p.Count() == 1 && p.Count(a => a.Id == "EURUSD") == 1)), Times.Once);
        }

        [TestMethod]
        public async Task Getting_all_items_first_time_updates_cache()
        {
            // Arrange
            _dateTimeProviderMock.SetupGet(p => p.UtcNow).Returns(() => new DateTime(2017, 06, 23, 21, 00, 00));
            _repositoryMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(() => new[]
                {
                    new TestItem {Id = "EURUSD"},
                });
            _cacheServiceMock.Setup(s => s.GetAll()).Returns(() => new TestItem[0]);

            // Act
            await _manager.GetAllAsync();

            // Asert
            _repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
            // ReSharper disable once PossibleMultipleEnumeration
            _cacheServiceMock.Verify(s => s.Update(It.Is<IEnumerable<TestItem>>(p => p.Count() == 1 && p.Count(a => a.Id == "EURUSD") == 1)), Times.Once);
        }

        [TestMethod]
        public async Task Getting_item_once_after_cache_expiration_updates_cache()
        {
            // Arrange
            var initialNow = new DateTime(2017, 06, 23, 21, 00, 00);

            _dateTimeProviderMock.SetupGet(p => p.UtcNow).Returns(() => initialNow);
            _repositoryMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(() => new []
                {
                    new TestItem {Id = "EURUSD"},
                });

            await _manager.TryGetAsync("EURUSD");

            _dateTimeProviderMock.SetupGet(p => p.UtcNow).Returns(() => initialNow.Add(_cacheExpirationPeriod).AddTicks(1));

            _repositoryMock.ResetCalls();
            _cacheServiceMock.ResetCalls();

            // Act
            await _manager.TryGetAsync("EURUSD");

            // Asert
            _repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
            // ReSharper disable once PossibleMultipleEnumeration
            _cacheServiceMock.Verify(s => s.Update(It.Is<IEnumerable<TestItem>>(p => p.Count() == 1 && p.Count(a => a.Id == "EURUSD") == 1)), Times.Once);
        }

        [TestMethod]
        public async Task Getting_all_items_once_after_cache_expiration_updates_cache()
        {
            // Arrange
            var initialNow = new DateTime(2017, 06, 23, 21, 00, 00);

            _dateTimeProviderMock.SetupGet(p => p.UtcNow).Returns(() => initialNow);
            _repositoryMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(() => new []
                {
                    new TestItem {Id = "EURUSD"},
                });
            _cacheServiceMock.Setup(s => s.GetAll()).Returns(() => new TestItem[0]);

            await _manager.TryGetAsync("EURUSD");

            _dateTimeProviderMock.SetupGet(p => p.UtcNow).Returns(() => initialNow.Add(_cacheExpirationPeriod).AddTicks(1));

            _repositoryMock.ResetCalls();
            _cacheServiceMock.ResetCalls();

            // Act
            await _manager.GetAllAsync();

            // Asert
            _repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
            // ReSharper disable once PossibleMultipleEnumeration
            _cacheServiceMock.Verify(s => s.Update(It.Is<IEnumerable<TestItem>>(p => p.Count() == 1 && p.Count(a => a.Id == "EURUSD") == 1)), Times.Once);
        }

        #endregion
    }
}