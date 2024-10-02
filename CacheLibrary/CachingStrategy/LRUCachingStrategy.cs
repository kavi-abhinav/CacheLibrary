using CacheLibrary.Models;

namespace CacheLibrary.CachingStrategy
{
    internal class LRUCachingStrategy : ICachingStrategy
    {
        private int _bucketSize;
        private Dictionary<string, CacheNode> _cacheNodeMap;
        private LinkedList<CacheNode> _cacheList;

        public LRUCachingStrategy(int bucketSize)
        {
            _bucketSize = bucketSize;
            _cacheNodeMap = new Dictionary<string, CacheNode>();
            _cacheList = new LinkedList<CacheNode>();
        }

        public void Add(string key, string value)
        {
            //Add before eviction, as add might not be successful
            AddToCache(key, value);

            //evict
            if(_cacheNodeMap.Count() == _bucketSize + 1)
            {
                CacheNode node = _cacheList.Last();
                _cacheNodeMap.Remove(node.Key);
                _cacheList.RemoveLast();
            }
        }

        private void AddToCache(string key, string value)
        {
            if (_cacheNodeMap.ContainsKey(key))
            {
                throw new ArgumentException($"Cache key {key} already exists");
            }
            else
            {
                CacheNode node = new(key, value);
                _cacheNodeMap.Add(key, node);
                _cacheList.AddFirst(node);
            }
        }

        public string Get(string key)
        {
            CacheNode? node;
            if ( _cacheNodeMap.TryGetValue(key, out node))
            {
                UpdateNodePosition(node);
                return node.Value;
            }
            else throw new ArgumentException($"Cache key {key} does not exists");
        }

        private void UpdateNodePosition(CacheNode node)
        {
            _cacheList.Remove(node);
            _cacheList.AddFirst(node);
        }

        public void Update(string key, string value)
        {
            CacheNode? node;
            if (_cacheNodeMap.TryGetValue(key, out node))
            {
                node.Value = value;
                UpdateNodePosition(node);
            }
            else throw new ArgumentException($"Cache key {key} does not exists");
        }
    }
}
