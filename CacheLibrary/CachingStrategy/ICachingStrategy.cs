namespace CacheLibrary.CachingStrategy
{
    internal interface ICachingStrategy
    {

        void Add(string key, string value);

        string Get(string key);

        void Update(string key, string value);
    }
}
