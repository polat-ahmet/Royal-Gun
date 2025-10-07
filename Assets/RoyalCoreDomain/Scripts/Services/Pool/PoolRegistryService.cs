using System;
using System.Collections.Generic;

namespace RoyalCoreDomain.Scripts.Services.Pool
{
    public sealed class PoolRegistryService : IPoolRegistryService, IDisposable
    {
        private readonly Dictionary<string, object> _map = new(32);

        public IPool<T> GetOrCreate<T>(PoolId id, Func<IPool<T>> create)
        {
            var k = id.ToString();
            if (_map.TryGetValue(k, out var o) && o is IPool<T> p) return p;
            var np = create();
            _map[k] = np;
            return np;
        }

        public bool TryGet<T>(PoolId id, out IPool<T> pool)
        {
            if (_map.TryGetValue(id.ToString(), out var o) && o is IPool<T> p) { pool = p; return true; }
            pool = default; return false;
        }

        public void Remove(PoolId id)
        {
            var k = id.ToString();
            if (_map.TryGetValue(k, out var o))
            {
                if (o is IDisposable d) d.Dispose();
                _map.Remove(k);
            }
        }

        public void Clear()
        {
            foreach (var o in _map.Values) if (o is IDisposable d) d.Dispose();
            _map.Clear();
        }

        public void Dispose() => Clear();
    }
}