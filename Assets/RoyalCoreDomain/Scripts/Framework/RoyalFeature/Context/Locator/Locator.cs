using System;
using System.Collections.Generic;

namespace RoyalCoreDomain.Scripts.Framework.RoyalFeature.Context.Locator
{
    public sealed class Locator<TBase> : ILocator<TBase> where TBase : class
    {
        private readonly Dictionary<Type, TBase> _map = new();

        public void Bind<T>(T instance) where T : class, TBase
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            _map[typeof(T)] = instance;
        }

        public bool TryGet<T>(out T instance) where T : class, TBase
        {
            if (_map.TryGetValue(typeof(T), out var obj))
            {
                instance = (T)obj;
                return true;
            }

            instance = null;
            return false;
        }

        public T Require<T>() where T : class, TBase
        {
            return TryGet<T>(out var s)
                ? s
                : throw new InvalidOperationException(
                    $"[Locator<{typeof(TBase).Name}>] Missing binding for {typeof(T).FullName}");
        }

        public void Unbind<T>() where T : class, TBase
        {
            _map.Remove(typeof(T));
        }

        public void Clear()
        {
            _map.Clear();
        }
    }

    public sealed class ListLocator<TBase> : IListLocator<TBase> where TBase : class
    {
        private readonly Dictionary<Type, List<TBase>> _map = new();

        public void Add<T>(T instance) where T : class, TBase
        {
            if (!_map.TryGetValue(typeof(T), out var list))
                _map[typeof(T)] = list = new List<TBase>(4);
            list.Add(instance);
        }

        public bool Remove<T>(T instance) where T : class, TBase
        {
            return _map.TryGetValue(typeof(T), out var list) && list.Remove(instance);
        }

        public IReadOnlyList<T> GetAll<T>() where T : class, TBase
        {
            if (_map.TryGetValue(typeof(T), out var list))
            {
                var result = new T[list.Count];
                for (var i = 0; i < list.Count; i++) result[i] = (T)list[i];
                return result;
            }

            return Array.Empty<T>();
        }

        public void Clear()
        {
            _map.Clear();
        }
    }

    public sealed class KeyedLocator<TKey, TBase> : IKeyedLocator<TKey, TBase>
        where TKey : notnull where TBase : class
    {
        private readonly Dictionary<Type, Dictionary<TKey, TBase>> _map = new();

        public bool Register<T>(TKey key, T instance) where T : class, TBase
        {
            if (!_map.TryGetValue(typeof(T), out var dict))
                _map[typeof(T)] = dict = new Dictionary<TKey, TBase>();
            if (dict.ContainsKey(key)) return false;
            dict[key] = instance;
            return true;
        }

        public bool Unregister<T>(TKey key) where T : class, TBase
        {
            return _map.TryGetValue(typeof(T), out var dict) && dict.Remove(key);
        }

        public bool TryGet<T>(TKey key, out T instance) where T : class, TBase
        {
            instance = null;
            if (_map.TryGetValue(typeof(T), out var dict) && dict.TryGetValue(key, out var v))
            {
                instance = (T)v;
                return true;
            }

            return false;
        }

        public IEnumerable<(TKey Key, T Instance)> All<T>() where T : class, TBase
        {
            if (_map.TryGetValue(typeof(T), out var dict))
                foreach (var kv in dict)
                    yield return (kv.Key, (T)kv.Value);
        }

        public void Clear()
        {
            _map.Clear();
        }
    }
}