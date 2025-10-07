using RoyalCoreDomain.Scripts.Framework.RoyalFeature.MVC.View;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Services;
using UnityEngine;

namespace RoyalCoreDomain.Scripts.Services.Providers.ViewProvider
{
    public interface IViewProvider : IService
    {
        T LoadView<T>(string key) where T : Component, IView; // sync

        void Release(Object obj);
    }
}