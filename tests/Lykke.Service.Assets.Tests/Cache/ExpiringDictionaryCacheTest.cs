using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lykke.Service.Assets.Client.Cache;
using Lykke.Service.Assets.Client.Updaters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Lykke.Service.Assets.Tests.Cache
{
    [TestClass]
    public class ExpiringDictionaryCacheTest
    {
        [TestMethod]
        public async Task TestExpiringCache()
        {
            var items = Enumerable.Range(1, 5).Select(x => new CacheItem { Id = x.ToString() });

            var updater = new Mock<IUpdater<CacheItem>>();
            updater.Setup(x => x.GetItemsAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(items));

            var sot = new ExpiringDictionaryCache<CacheItem>(TimeSpan.FromMilliseconds(100), updater.Object);

            var result1 = await sot.GetAll(new CancellationToken());
            Assert.IsNotNull(result1);
            Assert.AreEqual(5, result1.Count);

            updater.Verify(x => x.GetItemsAsync(It.IsAny<CancellationToken>()), Times.Once);

            var result2 = await sot.GetAll(new CancellationToken());
            Assert.AreSame(result1, result2);

            updater.Verify(x => x.GetItemsAsync(It.IsAny<CancellationToken>()), Times.Once);

            await Task.Delay(150);

            updater.Verify(x => x.GetItemsAsync(It.IsAny<CancellationToken>()), Times.Once, @"Cache should expire not auto update");

            var result3 = await sot.GetAll(new CancellationToken());
            Assert.IsNotNull(result3);
            Assert.AreEqual(5, result1.Count);
            Assert.AreNotSame(result1, result3);

            updater.Verify(x => x.GetItemsAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
        }

        public class CacheItem : ICacheItem
        {
            public string Id { get; set; }
        }
    }
}
