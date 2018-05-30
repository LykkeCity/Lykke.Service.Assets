using Lykke.Service.Assets.Client.Cache;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Common.Log;

namespace Lykke.Service.Assets.Tests.Cache
{
    [TestClass]
    public class DictionaryCacheTest
    {
        private int _getItemsCallCount;

        [TestInitialize]
        public void Setup()
        {
            _getItemsCallCount = 0;
        }

        [TestMethod]
        public async Task TestExpiringCache()
        {
            var date = DateTime.UtcNow;
            var provider = new Mock<IDateTimeProvider>();
            provider.SetupGet(x => x.UtcNow).Returns(date);

            var sot = new DictionaryCache<CacheItem>(provider.Object, TimeSpan.FromSeconds(30));

            await sot.EnsureCacheIsUpdatedAsync(GetItems);

            Assert.IsTrue(sot.GetAll().Count > 0);
            Assert.AreEqual(1, _getItemsCallCount);

            provider.SetupGet(x => x.UtcNow).Returns(date.AddMinutes(1));
            await sot.EnsureCacheIsUpdatedAsync(GetItems);
            Assert.AreEqual(2, _getItemsCallCount);

            provider.SetupGet(x => x.UtcNow).Returns(date.AddSeconds(29));

            await sot.EnsureCacheIsUpdatedAsync(GetItems);
            Assert.AreEqual(2, _getItemsCallCount);
        }

        [TestMethod]
        public async Task TestRefreshingCache()
        {
            var date = DateTime.UtcNow;
            var provider = new Mock<IDateTimeProvider>();
            provider.SetupGet(x => x.UtcNow).Returns(date);

            var sot = new DictionaryCache<CacheItem>(provider.Object, TimeSpan.FromMilliseconds(100));
            using (sot.StartAutoUpdate("TestRefreshingCache", Mock.Of<ILog>(), GetItems))
            {
                Assert.IsTrue(sot.GetAll().Count > 0, "Dictionary should be filled after update");
                Assert.AreEqual(1, _getItemsCallCount, "Initial update should be called at start");

                await Task.Delay(150);
                
                Assert.AreEqual(2, _getItemsCallCount, "Update should have been called after timer tick");
            }

            await Task.Delay(150);

            Assert.AreEqual(2, _getItemsCallCount, "Auto update should have stopped after dispose");
        }

        private Task<IEnumerable<CacheItem>> GetItems()
        {
            _getItemsCallCount++;

            IEnumerable<CacheItem> result = new List<CacheItem>
            {
                new CacheItem {Id = "1"},
                new CacheItem {Id = "2"}
            };

            return Task.FromResult(result);
        }

        private class CacheItem : ICacheItem
        {
            public string Id { get; set; }
        }
    }
}
