using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheLibrary.UnitTests
{
    public class LFUCacheTests
    {
        private CacheClient? _cacheClient;


        [Fact]
        public void CacheExpiresKeysBasedOnLRULogicIfFrequencyIsZero()
        {
            //setup
            _cacheClient = new CacheClient(CachingStrategyOptions.LFU, 3);
            _cacheClient.Add("1", "v1");
            _cacheClient.Add("2", "v2");
            _cacheClient.Add("3", "v3");

            _cacheClient.Add("4", "v4");

            Assert.Throws<ArgumentException>(() => _cacheClient.Get("1")); //1 should have been evicted as per LRU as freq is same
        }

        [Fact]
        public void CacheExpiresKeysBasedOnLRULogicIfFrequencyIsSame()
        {
            //setup
            _cacheClient = new CacheClient(CachingStrategyOptions.LFU, 3);
            _cacheClient.Add("1", "v1");
            _cacheClient.Add("2", "v2");
            _cacheClient.Add("3", "v3");

            _cacheClient.Get("1");
            _cacheClient.Get("2");
            _cacheClient.Get("3");
            _cacheClient.Get("2");
            _cacheClient.Get("3");
            _cacheClient.Get("1");

            _cacheClient.Add("4", "v4");

            Assert.Throws<ArgumentException>(() => _cacheClient.Get("2")); //2 should have been evicted as per LRU as freq is same
        }

        [Fact]
        public void CacheExpiresKeysBasedOnLFULogicOnGet()
        {
            //setup
            _cacheClient = new CacheClient(CachingStrategyOptions.LFU, 3);
            _cacheClient.Add("1", "v1");
            _cacheClient.Add("2", "v2");
            _cacheClient.Add("3", "v3");
            _cacheClient.Get("2");
            _cacheClient.Get("3");
            _cacheClient.Get("2");
            _cacheClient.Get("3");
            _cacheClient.Get("1");


            _cacheClient.Add("4", "v4");

            Assert.Throws<ArgumentException>(() => _cacheClient.Get("1")); //1 should have been evicted as per LFU
        }

        [Fact]
        public void CacheExpiresKeysBasedOnLFULogicOnUpdates()
        {
            //setup
            _cacheClient = new CacheClient(CachingStrategyOptions.LFU, 3);
            _cacheClient.Add("1", "v1");
            _cacheClient.Add("2", "v2");
            _cacheClient.Add("3", "v3");
            _cacheClient.Update("2", "v2.1");
            _cacheClient.Update("2", "v2.2");
            _cacheClient.Update("1", "v1.1");
            _cacheClient.Update("1", "v1.2");
            _cacheClient.Update("3", "v3.1");


            _cacheClient.Add("4", "v4");

            Assert.Throws<ArgumentException>(() => _cacheClient.Get("3")); //3 should have been evicted as per LFU
        }
    }
}
