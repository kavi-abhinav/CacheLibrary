namespace CacheLibrary
{
    internal static class CachingStrategyFactory
    {
        internal static ICachingStrategy CreateStrategy(CachingStrategyOptions option, int bucketSize)
        {
            ICachingStrategy strategy;
            switch (option) {

                case CachingStrategyOptions.FIFO:
                    strategy = new FIFOCachingStrategy(bucketSize);
                    break;
                default:
                    strategy = new FIFOCachingStrategy(bucketSize); //defaults to fifo strategy
                    break;

            }

            return strategy;
        }
    }
}
