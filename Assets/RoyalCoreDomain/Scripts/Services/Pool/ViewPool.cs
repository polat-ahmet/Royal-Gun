using System;
using System.Collections.Generic;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Context;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.MVC.View;
using RoyalCoreDomain.Scripts.Services.Providers.ViewProvider;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RoyalCoreDomain.Scripts.Services.Pool
{
    public sealed class ViewPool<TView> : IPool<TView>, IDisposable
    where TView : Component, IView, IPoolable
{
    private readonly string _viewKey;           // "Bullet/Views/BulletView"
    private readonly Transform _holder;         // Parent object holds pooled objects
    private readonly PoolConfig _cfg;
    private readonly IViewProvider _views;
    private readonly Stack<TView> _stack = new();
    private int _created;

    public ViewPool(IViewProvider vp, string viewKey, Transform holder, PoolConfig cfg = null)
    {
        _viewKey = viewKey; _holder = holder;
        _cfg = cfg ?? new PoolConfig();
        _views = vp;

        // prewarm (sync)
        for (int i = 0; i < _cfg.InitialAmount; i++)
        {
            var v = CreateOne();
            Park(v);
            _stack.Push(v);
        }
    }

    public int Count => _stack.Count;

    public TView Rent()
    {
        var v = _stack.Count > 0 ? _stack.Pop() : CreateOne();
        v.OnRent();
        v.transform.SetParent(null, false);
        v.gameObject.SetActive(true);
        return v;
    }

    public void Return(TView v)
    {
        if (!v) return;
        v.OnReturn();
        Park(v);
        _stack.Push(v);
    }

    public void Dispose()
    {
        while (_stack.Count > 0)
        {
            var v = _stack.Pop();
            if (v) Object.Destroy(v.gameObject);
        }
    }

    private TView CreateOne()
    {
        if (_cfg.StrictCap && _created >= _cfg.MaxSize)
            throw new System.InvalidOperationException($"[ViewPool] MaxSize reached for '{_viewKey}'");
        var v = _views.LoadView<TView>(_viewKey);
        _created++;
        return v;
    }

    private void Park(TView v)
    {
        v.gameObject.SetActive(false);
        if (_holder) v.transform.SetParent(_holder, false);
    }
}
}