namespace RoyalCoreDomain.Scripts.Services.Pool
{
    public interface IPoolable
    {
        void OnRent();
        void OnReturn();
    }
}