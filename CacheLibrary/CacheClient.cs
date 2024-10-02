namespace CacheLibrary
{
    public class CacheClient
    {
        private readonly int _bucketSize;
        private readonly ICachingStrategy _cachingStrategy;

        public CacheClient(CachingStrategyOptions cachingStrategyOption)
        {
            _bucketSize = 10; //default bucket size
            _cachingStrategy = CachingStrategyFactory.CreateStrategy(cachingStrategyOption, _bucketSize);
        }

        public CacheClient(CachingStrategyOptions cachingStrategyOption, int bucketSize)
        {
            _bucketSize = bucketSize;
            _cachingStrategy = CachingStrategyFactory.CreateStrategy(cachingStrategyOption, _bucketSize);
        }


        public void Add(string key, string value)
        {
            _cachingStrategy.Add(key, value);
        }

        public string Get(string key)
        {
            return _cachingStrategy.Get(key);
        }

    }
}
