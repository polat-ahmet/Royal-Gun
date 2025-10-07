using System.Collections.Generic;

namespace RoyalCoreDomain.Scripts.Framework.RoyalFeature.Context.Locator
{
    // Single instance each type
    public interface ILocator<TBase> where TBase : class
    {
        void Bind<T>(T instance) where T : class, TBase;
        bool TryGet<T>(out T instance) where T : class, TBase;
        T Require<T>() where T : class, TBase;
        void Unbind<T>() where T : class, TBase;
        void Clear();
    }

    // Multiple instance same type
    public interface IListLocator<TBase> where TBase : class
    {
        void Add<T>(T instance) where T : class, TBase;
        bool Remove<T>(T instance) where T : class, TBase;
        IReadOnlyList<T> GetAll<T>() where T : class, TBase;
        void Clear();
    }

    // Keyed (directory) — id -> instance
    public interface IKeyedLocator<TKey, TBase> where TKey : notnull where TBase : class
    {
        bool Register<T>(TKey key, T instance) where T : class, TBase;
        bool Unregister<T>(TKey key) where T : class, TBase;
        bool TryGet<T>(TKey key, out T instance) where T : class, TBase;
        IEnumerable<(TKey Key, T Instance)> All<T>() where T : class, TBase;
        void Clear();
    }
}