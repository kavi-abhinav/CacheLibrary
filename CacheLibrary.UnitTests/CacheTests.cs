namespace CacheLibrary.UnitTests
{
    public class CacheTests
    {
        private CacheClient? _cacheClient;

        //Validation Tests

        [Theory]
        [InlineData(CachingStrategyOptions.FIFO, null)]
        [InlineData(CachingStrategyOptions.FIFO, " ")]
        [InlineData(CachingStrategyOptions.LRU, null)]
        [InlineData(CachingStrategyOptions.LRU, " ")]
        [InlineData(CachingStrategyOptions.LFU, null)]
        [InlineData(CachingStrategyOptions.LFU, " ")]
        public void CacheThrowsArgumentExceptionDuringAddIfKeyIsNullOrEmpty(CachingStrategyOptions cacheStrategy, string key)
        {
            //setup
            _cacheClient = _cacheClient = new CacheClient(cacheStrategy);

            //execute and verify
            Assert.ThrowsAny<ArgumentException>(() => _cacheClient.Add(key, "value1"));

        }

        [Theory]
        [InlineData(CachingStrategyOptions.FIFO, null)]
        [InlineData(CachingStrategyOptions.FIFO, " ")]
        [InlineData(CachingStrategyOptions.LRU, null)]
        [InlineData(CachingStrategyOptions.LRU, " ")]
        [InlineData(CachingStrategyOptions.LFU, null)]
        [InlineData(CachingStrategyOptions.LFU, " ")]
        public void CacheThrowsArgumentExceptionDuringUpdateIfKeyIsNullOrEmpty(CachingStrategyOptions cacheStrategy, string key)
        {
            //setup
            _cacheClient = _cacheClient = new CacheClient(cacheStrategy);

            //execute and verify
            Assert.ThrowsAny<ArgumentException>(() => _cacheClient.Update(key, "value1"));

        }

        [Theory]
        [InlineData(CachingStrategyOptions.FIFO, null)]
        [InlineData(CachingStrategyOptions.FIFO, " ")]
        [InlineData(CachingStrategyOptions.LRU, null)]
        [InlineData(CachingStrategyOptions.LRU, " ")]
        [InlineData(CachingStrategyOptions.LFU, null)]
        [InlineData(CachingStrategyOptions.LFU, " ")]
        public void CacheThrowsArgumentExceptionDuringGetIfKeyIsNullOrEmpty(CachingStrategyOptions cacheStrategy, string key)
        {
            //setup
            _cacheClient = _cacheClient = new CacheClient(cacheStrategy);

            //execute and verify
            Assert.ThrowsAny<ArgumentException>(() => _cacheClient.Get(key));
        }


        [Theory]
        [InlineData(CachingStrategyOptions.FIFO, null)]
        [InlineData(CachingStrategyOptions.FIFO, " ")]
        [InlineData(CachingStrategyOptions.LRU, null)]
        [InlineData(CachingStrategyOptions.LRU, " ")]
        [InlineData(CachingStrategyOptions.LFU, null)]
        [InlineData(CachingStrategyOptions.LFU, " ")]
        public void CacheThrowsArgumentExceptionDuringAddIfValueIsNullOrEmpty(CachingStrategyOptions cacheStrategy, string value)
        {
            //setup
            _cacheClient = _cacheClient = new CacheClient(cacheStrategy);

            //execute and verify
            Assert.ThrowsAny<ArgumentException>(() => _cacheClient.Add("test-key", value));

        }


        [Theory]
        [InlineData(CachingStrategyOptions.FIFO, null)]
        [InlineData(CachingStrategyOptions.FIFO, " ")]
        [InlineData(CachingStrategyOptions.LRU, null)]
        [InlineData(CachingStrategyOptions.LRU, " ")]
        [InlineData(CachingStrategyOptions.LFU, null)]
        [InlineData(CachingStrategyOptions.LFU, " ")]
        public void CacheThrowsArgumentExceptionDuringUpdateIfValueIsNullOrEmpty(CachingStrategyOptions cacheStrategy, string value)
        {
            //setup
            _cacheClient = _cacheClient = new CacheClient(cacheStrategy);

            //execute and verify
            Assert.ThrowsAny<ArgumentException>(() => _cacheClient.Update("test-key", value));

        }

        [Theory]
        [InlineData(CachingStrategyOptions.FIFO, "name", "Abhinav")]
        [InlineData(CachingStrategyOptions.LRU, "name", "Abhinav")]
        [InlineData(CachingStrategyOptions.LFU, "name", "Abhinav")]
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


        //Functionality tests

        [Theory]
        [InlineData(CachingStrategyOptions.FIFO)]
        [InlineData(CachingStrategyOptions.LRU)]
        [InlineData(CachingStrategyOptions.LFU)]
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
        [InlineData(CachingStrategyOptions.LFU)]
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
        [InlineData(CachingStrategyOptions.LFU)]
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
        [InlineData(CachingStrategyOptions.LFU,"id", "1", "2")]
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
