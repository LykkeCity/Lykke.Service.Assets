using Lykke.Service.Assets.Client.Cache;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.Assets.Client.Updaters;

namespace Lykke.Service.Assets.Tests.Cache
{
    [TestClass]
    public class RefreshingDictionaryCacheTest
    {
        [TestMethod]
        public async Task TestRefreshingCache()
        {
            var items = Enumerable.Range(1, 5).Select(x => new CacheItem { Id = x.ToString() });

            var updater = new Mock<IUpdater<CacheItem>>();
            updater.Setup(x => x.GetItemsAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(items));

            var sot = new RefreshingDictionaryCache<CacheItem>(TimeSpan.FromMilliseconds(200), updater.Object, Mock.Of<ILog>());

            // Give system some time to update in background
            await Task.Delay(50);

            updater.Verify(x => x.GetItemsAsync(It.IsAny<CancellationToken>()), Times.Once, @"Cache should auto initialize");

            var result1 = await sot.GetAll(new CancellationToken());
            Assert.IsNotNull(result1);
            Assert.AreEqual(5, result1.Count);

            var result2 = await sot.GetAll(new CancellationToken());
            Assert.AreSame(result1, result2);

            updater.Verify(x => x.GetItemsAsync(It.IsAny<CancellationToken>()), Times.Once, @"Cache should not update on GetItems");

            await Task.Delay(250);

            updater.Verify(x => x.GetItemsAsync(It.IsAny<CancellationToken>()), Times.Exactly(2), @"Cache should update in background");

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
