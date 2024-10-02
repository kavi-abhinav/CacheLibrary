namespace CacheLibrary.UnitTests
{
    public class FIFOCacheTests
    {
        private CacheClient? _cacheClient;


        [Theory]
        [InlineData("name","Abhinav")]
        [InlineData("steps", "500")]
        [InlineData("occupation", "Software Engineer")]
        public void CacheReturnsCorrectValue(string key, string value)
        {
            //setup
            _cacheClient = new CacheClient(CachingStrategyOptions.FIFO);
            _cacheClient.Add(key, value);

            //execute
            string fetchedValue = _cacheClient.Get(key);

            //verify
            Assert.Equal(value, fetchedValue);

        }

        [Fact]
        public void CacheThrowsExceptionIfKeyAlreadyExists()
        {
            //setup
            _cacheClient = new CacheClient(CachingStrategyOptions.FIFO);
            _cacheClient.Add("test-key","value1");

            //execute and verify
            Assert.Throws<ArgumentException>(() => _cacheClient.Add("test-key", "value2"));

        }

        [Fact]
        public void CacheThrowsExceptionIfKeyNotFound()
        { 
            //setup
            _cacheClient = new CacheClient(CachingStrategyOptions.FIFO);

            //execute and verify
            Assert.Throws<ArgumentException>(() => _cacheClient.Get("non-existing-key"));

        }


        [Fact]
        public void CacheThrowsExceptionDuringUpdateIfKeyNotFound()
        {
            //setup
            _cacheClient = new CacheClient(CachingStrategyOptions.FIFO);

            //execute and verify
            Assert.Throws<ArgumentException>(() => _cacheClient.Update("non-existing-key", "hello"));

        }

        [Theory]
        [InlineData("id","1","2")]
        [InlineData("name", "abhinav", "Abhinav")]
        public void CacheCorrectlyUpdatesValue(string key, string value, string updatedValue)
        {
            //setup
            _cacheClient = new CacheClient(CachingStrategyOptions.FIFO);
            _cacheClient.Add(key, value);

            //execute
            string fetchedValue = _cacheClient.Get(key);

            //verify
            Assert.Equal(value, fetchedValue);

            //excecute
            _cacheClient.Update(key,updatedValue);
            fetchedValue = _cacheClient.Get(key);

            //verify
            Assert.Equal(updatedValue, fetchedValue);
        }

        [Fact]
        public void CacheExpiresKeysBasedOnFIFOLogic()
        {
            //setup
            _cacheClient = new CacheClient(CachingStrategyOptions.FIFO, 3);
            _cacheClient.Add("1", "v1");
            _cacheClient.Add("2", "v2");
            _cacheClient.Add("3", "v3");

            string fetchedValue = _cacheClient.Get("1");
            Assert.Equal("v1", fetchedValue);

            _cacheClient.Add("4", "v4");

            Assert.Throws<ArgumentException>(() => _cacheClient.Get("1"));
        }
    }
}