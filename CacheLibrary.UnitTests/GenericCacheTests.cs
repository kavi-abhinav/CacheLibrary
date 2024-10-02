using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheLibrary.UnitTests
{
    public class GenericCacheTests
    {
        private CacheClient? _cacheClient;


        [Theory]
        [InlineData(CachingStrategyOptions.FIFO, "name", "Abhinav")]
        [InlineData(CachingStrategyOptions.LRU, "name", "Abhinav")]
        public void CacheReturnsCorrectValue(CachingStrategyOptions cacheStrategy, string key, string value)
        {
            //setup
            _cacheClient = new CacheClient(cacheStrategy);
            _cacheClient.Add(key, value);

            //execute
            string fetchedValue = _cacheClient.Get(key);

            //verify
            Assert.Equal(value, fetchedValue);

        }



        [Theory]
        [InlineData(CachingStrategyOptions.FIFO)]
        [InlineData(CachingStrategyOptions.LRU)]
        public void CacheThrowsExceptionIfKeyAlreadyExists(CachingStrategyOptions cacheStrategy)
        {
            //setup
            _cacheClient = _cacheClient = new CacheClient(cacheStrategy);
            _cacheClient.Add("test-key", "value1");

            //execute and verify
            Assert.Throws<ArgumentException>(() => _cacheClient.Add("test-key", "value2"));

        }

        [Theory]
        [InlineData(CachingStrategyOptions.FIFO)]
        [InlineData(CachingStrategyOptions.LRU)]
        public void CacheThrowsExceptionIfKeyNotFound(CachingStrategyOptions cacheStrategy)
        {
            //setup
            _cacheClient = _cacheClient = new CacheClient(cacheStrategy);

            //execute and verify
            Assert.Throws<ArgumentException>(() => _cacheClient.Get("non-existing-key"));

        }


        [Theory]
        [InlineData(CachingStrategyOptions.FIFO)]
        [InlineData(CachingStrategyOptions.LRU)]
        public void CacheThrowsExceptionDuringUpdateIfKeyNotFound(CachingStrategyOptions cacheStrategy)
        {
            //setup
            _cacheClient = new CacheClient(cacheStrategy);

            //execute and verify
            Assert.Throws<ArgumentException>(() => _cacheClient.Update("non-existing-key", "hello"));

        }

        [Theory]
        [InlineData(CachingStrategyOptions.FIFO,"id", "1", "2")]
        [InlineData(CachingStrategyOptions.LRU,"id", "1", "2")]
        public void CacheCorrectlyUpdatesValue(CachingStrategyOptions cacheStrategy, string key, string value, string updatedValue)
        {
            //setup
            _cacheClient = new CacheClient(cacheStrategy);
            _cacheClient.Add(key, value);

            //execute
            string fetchedValue = _cacheClient.Get(key);

            //verify
            Assert.Equal(value, fetchedValue);

            //excecute
            _cacheClient.Update(key, updatedValue);
            fetchedValue = _cacheClient.Get(key);

            //verify
            Assert.Equal(updatedValue, fetchedValue);
        }
    }
}
