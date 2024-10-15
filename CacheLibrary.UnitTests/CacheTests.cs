using System.Text.Json;

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


        //Generic Tests

        [Theory]
        [InlineData(CachingStrategyOptions.FIFO)]
        [InlineData(CachingStrategyOptions.LRU)]
        [InlineData(CachingStrategyOptions.LFU)]
        public void CacheWorksWithIntegersAsExpected(CachingStrategyOptions cacheStrategy)
        {
            //setup
            _cacheClient = new CacheClient(cacheStrategy);

            //execute 
            _cacheClient.Add("input", 1);
            var fetchedValue = _cacheClient.Get<int>("input");

            //verify
            Assert.IsType<int>(fetchedValue);
            Assert.Equal(1, fetchedValue);

            _cacheClient.Update("input", 2);
            fetchedValue = _cacheClient.Get<int>("input");

            //verify
            Assert.IsType<int>(fetchedValue);
            Assert.Equal(2, fetchedValue);


        }


        [Theory]
        [InlineData(CachingStrategyOptions.FIFO)]
        [InlineData(CachingStrategyOptions.LRU)]
        [InlineData(CachingStrategyOptions.LFU)]
        public void CacheWorksWithBooleanAsExpected(CachingStrategyOptions cacheStrategy)
        {
            //setup
            _cacheClient = new CacheClient(cacheStrategy);

            //execute 
            _cacheClient.Add("input", true);
            var fetchedValue = _cacheClient.Get<bool>("input");

            //verify
            Assert.IsType<bool>(fetchedValue);
            Assert.True(fetchedValue);

            _cacheClient.Update("input", false);
            fetchedValue = _cacheClient.Get<bool>("input");

            //verify
            Assert.IsType<bool>(fetchedValue);
            Assert.False(fetchedValue);

        }


        [Theory]
        [InlineData(CachingStrategyOptions.FIFO)]
        [InlineData(CachingStrategyOptions.LRU)]
        [InlineData(CachingStrategyOptions.LFU)]
        public void CacheWorksWithObjectAsExpected(CachingStrategyOptions cacheStrategy)
        {
            //setup
            _cacheClient = new CacheClient(cacheStrategy);
            TestEmployee newEmployee = new()
            {
                Id = "1",
                Name = "Abhinav"
            };

            //execute 
            _cacheClient.Add("input", newEmployee);
            var fetchedValue = _cacheClient.Get<TestEmployee>("input");

            //verify
            Assert.IsType<TestEmployee>(fetchedValue);
            Assert.Equal(newEmployee, fetchedValue);


            newEmployee.Name = "Rahul";
            _cacheClient.Update("input", newEmployee);
            fetchedValue = _cacheClient.Get<TestEmployee>("input");

            //verify
            Assert.IsType<TestEmployee>(fetchedValue);
            Assert.Equal(newEmployee, fetchedValue);

        }

        [Theory]
        [InlineData(CachingStrategyOptions.FIFO)]
        [InlineData(CachingStrategyOptions.LRU)]
        [InlineData(CachingStrategyOptions.LFU)]
        public void CacheThrowsJsonExceptionWhenFetchingWithIncorrectType(CachingStrategyOptions cacheStrategy)
        {
            //setup
            _cacheClient = new CacheClient(cacheStrategy);
            TestEmployee newEmployee = new()
            {
                Id = "1",
                Name = "Abhinav"
            };

            //execute and verify
            _cacheClient.Add("input", newEmployee);
            Assert.Throws<JsonException>(() => _cacheClient.Get<int>("input"));
        }
    }

    public class TestEmployee
    {
        public string? Id { get;  set; }

        public string? Name { get; set; }

        public override bool Equals(Object? obj)
        {
            if (obj == null || !(obj is TestEmployee))
                return false;

            return  this.Id == ((TestEmployee)obj).Id && this.Name == ((TestEmployee)obj).Name;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
