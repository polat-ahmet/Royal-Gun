using RoyalCoreDomain.Scripts.Framework.RoyalFeature.MVC.View;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Services;
using UnityEngine;

namespace RoyalCoreDomain.Scripts.Services.UI
{
    public interface IUIService : IService
    {
        T Show<T>(string key, UILayer layer = UILayer.HUD)
            where T : Component, IView;

        void Close<T>(T view) where T : Component, IView;
        void CloseAll(UILayer layer);
        T PushPopup<T>(string key) where T : Component, IView;
        void PopPopup();
    }
}