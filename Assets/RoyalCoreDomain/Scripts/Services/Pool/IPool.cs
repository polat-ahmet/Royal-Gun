using System.Threading;
using System.Threading.Tasks;

namespace RoyalCoreDomain.Scripts.Services.Pool
{
    public interface IPool<T>
    {
        T Rent();
        void Return(T item);
        int Count { get; }
    }
}