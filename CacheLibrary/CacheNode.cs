namespace CacheLibrary
{
    internal class CacheNode
    {
        public CacheNode(string key, string value)
        {
            Key = key;
            Value = value;
        }

        internal string Key { get; set; }

        internal string Value { get; set; }
    }
}
