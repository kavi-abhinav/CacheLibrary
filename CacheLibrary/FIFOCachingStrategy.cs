namespace CacheLibrary
{
    internal class FIFOCachingStrategy : ICachingStrategy
    {
        private int _bucketSize;
        private Dictionary<string, CacheNode> _cacheNodeMap;
        private LinkedList<CacheNode> _cacheList;

        public FIFOCachingStrategy(int bucketSize)
        {
            _bucketSize = bucketSize;
            _cacheNodeMap = new Dictionary<string, CacheNode>();
            _cacheList = new LinkedList<CacheNode>();
        }

        public void Add(string key, string value)
        {
            //Adding to cache first as it might be unsuccesful
            AddToCache(key,value);

            if (_cacheNodeMap.Count == _bucketSize+1)
            {
                CacheNode firstNode = _cacheList.First();
                _cacheNodeMap.Remove(firstNode.Key);
                _cacheList.RemoveFirst();
            }
        }

        private void AddToCache(string key, string value)
        {
            CacheNode node = new(key, value);
            if (_cacheNodeMap.TryAdd(key, node))
            {
                _cacheList.AddLast(node);
            }
            else throw new ArgumentException($"Cache Key - '{key}' already exists. Cache keys are supposed to be unique.");
        }

        public string Get(string key)
        {
            CacheNode? node;
            if (_cacheNodeMap.TryGetValue(key, out node))
            {
                return node.Value;
            }
            else throw new ArgumentException($"Cache Key - '{key}' not found.");
        }

        public IEnumerable<string> PeekContent()
        {
            var enumerator = _cacheList.GetEnumerator();
            var contents = new List<string>();
            while (enumerator.MoveNext())
            {
                contents.Add($"{enumerator.Current.Key} = {enumerator.Current.Value}");
            }

            return contents;
        }

    }
}
