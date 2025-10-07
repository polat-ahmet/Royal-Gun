namespace RoyalCoreDomain.Scripts.Services.Pool
{
    public sealed class PoolConfig
    {
        public int InitialAmount      = 0;
        public int MaxSize      = 32;  // maximum size that can be created and stored
        public bool StrictCap   = false;// true→not create above MaxSize(Throw), false→create infinity
    }
}