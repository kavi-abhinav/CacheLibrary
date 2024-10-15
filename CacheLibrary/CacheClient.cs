using CacheLibrary.CachingStrategy;
using CacheLibrary.Factories;
using System.Text.Json;
using System.Text.Json.Serialization;

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
            ArgumentException.ThrowIfNullOrWhiteSpace(key, nameof(key));
            ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));

            _cachingStrategy.Add(key, value);
        }

        public string Get(string key)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(key, nameof(key));

            return _cachingStrategy.Get(key);
        }

        public void Update(string key, string value)
        {

            ArgumentException.ThrowIfNullOrWhiteSpace(key, nameof(key));
            ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));

            _cachingStrategy.Update(key, value);
        }

        public void Add<T>(string key, T value)
        {
            var serializedString = JsonSerializer.Serialize(value);
            Add(key, serializedString);
        }

        public T Get<T>(string key)
        {
            var serializedString = Get(key);
            var desializedValue = JsonSerializer.Deserialize<T>(serializedString);

            return desializedValue == null
                ? throw new Exception($"Cannot retrieve value for the Cache key - '{key}'")
                : desializedValue;
        }

        public void Update<T>(string key, T value)
        {
            var serializedString = JsonSerializer.Serialize(value, options: new JsonSerializerOptions { IncludeFields = true });
            Update(key, serializedString);
        }
    }
}
