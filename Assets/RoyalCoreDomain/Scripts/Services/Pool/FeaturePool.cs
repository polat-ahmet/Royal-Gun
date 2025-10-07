using System;
using System.Collections.Generic;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Feature;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Services.FeatureFactory;

namespace RoyalCoreDomain.Scripts.Services.Pool
{
    public sealed class FeaturePool<TFeature> : IPool<TFeature>, IDisposable
    where TFeature : BaseFeature, IPoolable
{
    private readonly IFeature _parent;
    private readonly Func<IFeature, TFeature> _create; // (parent) => new EnemyFeature("enemy", parent)
    private readonly PoolConfig _cfg;
    private readonly Stack<TFeature> _stack = new(64);
    private int _created;
    private IFeatureFactory _factory;

    public FeaturePool(IFeature parent, Func<IFeature, TFeature> create, IFeatureFactory factory, PoolConfig cfg = null)
    {
        _parent = parent; _create = create; _cfg = cfg ?? new PoolConfig();
        _factory = factory;

        // prewarm (sync)
        for (int i = 0; i < _cfg.InitialAmount; i++)
        {
            var f = CreateOne();
            Park(f);
            _stack.Push(f);
        }
    }

    public int Count => _stack.Count;

    public TFeature Rent()
    {
        var f = _stack.Count > 0 ? _stack.Pop() : CreateOne();
        if (f.State != FeatureState.Started) f.Start();
        f.OnRent();
        return f;
    }

    public void Return(TFeature f)
    {
        if (f == null) return;
        f.OnReturn();
        if (f.State == FeatureState.Started) f.Stop();
        Park(f);
        _stack.Push(f);
    }

    public void Dispose()
    {
        while (_stack.Count > 0)
        {
            var f = _stack.Pop();
            f.Dispose();
            f.Parent?.RemoveChild(f);
        }
    }

    private TFeature CreateOne()
    {
        if (_cfg.StrictCap && _created >= _cfg.MaxSize)
            throw new System.InvalidOperationException($"[FeaturePool] MaxSize reached for '{typeof(TFeature).Name}'");

        var f = _create(_parent);
        f.PreInstall(); f.Install(); f.Resolve(); // park (no Start)
        _created++;
        return f;
    }

    private void Park(TFeature f)
    {
        
    }
}
}