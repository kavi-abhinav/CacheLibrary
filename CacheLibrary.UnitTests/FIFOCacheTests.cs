namespace CacheLibrary.UnitTests
{
    public class FIFOCacheTests
    {
        private CacheClient? _cacheClient;

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


        [Fact]
        public void CacheExpiresFirstInKeyIrrespectiveOfGetOperation()
        {
            //setup
            _cacheClient = new CacheClient(CachingStrategyOptions.FIFO, 3);
            _cacheClient.Add("1", "v1");
            _cacheClient.Add("2", "v2");
            _cacheClient.Add("3", "v3");

            string fetchedValue = _cacheClient.Get("1");

            Assert.Equal("v1", fetchedValue); //get was successful

            _cacheClient.Add("4", "v4");

            Assert.Throws<ArgumentException>(() => _cacheClient.Get("1"));
        }


        [Fact]
        public void CacheExpiresFirstInKeyIrrespectiveOfUpdateOperation()
        {
            //setup
            _cacheClient = new CacheClient(CachingStrategyOptions.FIFO, 3);
            _cacheClient.Add("1", "v1");
            _cacheClient.Add("2", "v2");
            _cacheClient.Add("3", "v3");

            _cacheClient.Update("1", "v1.1");
            _cacheClient.Update("1", "v1.2");
            _cacheClient.Update("1", "v1.3");

            string fetchedValue = _cacheClient.Get("1");
            Assert.Equal("v1.3", fetchedValue); //Update was successful

            _cacheClient.Add("4", "v4");

            Assert.Throws<ArgumentException>(() => _cacheClient.Get("1"));
        }
    }
}