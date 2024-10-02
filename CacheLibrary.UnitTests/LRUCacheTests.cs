using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheLibrary.UnitTests
{
    public class LRUCacheTests
    {
        private CacheClient? _cacheClient;


        [Fact]
        public void CacheExpiresKeysBasedOnLRULogic()
        {
            //setup
            _cacheClient = new CacheClient(CachingStrategyOptions.LRU, 3);
            _cacheClient.Add("1", "v1");
            _cacheClient.Add("2", "v2");
            _cacheClient.Add("3", "v3");

            _cacheClient.Add("4", "v4");

            Assert.Throws<ArgumentException>(() => _cacheClient.Get("1")); //1 should have been evicted as per LRU
        }

        [Fact]
        public void CacheExpiresKeysBasedOnLRULogicOnGet()
        {
            //setup
            _cacheClient = new CacheClient(CachingStrategyOptions.LRU, 3);
            _cacheClient.Add("1", "v1");
            _cacheClient.Add("2", "v2");
            _cacheClient.Add("3", "v3");
            _cacheClient.Get("1");


            _cacheClient.Add("4", "v4");

            Assert.Throws<ArgumentException>(() => _cacheClient.Get("2")); //2 should have been evicted as per LRU
        }

        [Fact]
        public void CacheExpiresKeysBasedOnLRULogicOnUpdates()
        {
            //setup
            _cacheClient = new CacheClient(CachingStrategyOptions.LRU, 3);
            _cacheClient.Add("1", "v1");
            _cacheClient.Add("2", "v2");
            _cacheClient.Add("3", "v3");
            _cacheClient.Update("1", "v1.1");


            _cacheClient.Add("4", "v4");

            Assert.Throws<ArgumentException>(() => _cacheClient.Get("2")); //2 should have been evicted as per LRU
        }

    }
}
