namespace CacheLibrary
{
    internal interface ICachingStrategy
    {

        void Add(string key, string value);

        string Get(string key);

        IEnumerable<string> PeekContent();
    }
}
