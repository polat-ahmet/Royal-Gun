using RoyalCoreDomain.Scripts.Framework.RoyalFeature.MVC.View;
using UnityEngine;

namespace RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Input.Scripts.Views
{
    public interface IJoystickView : IView
    {
        RectTransform Outline { get; }
        RectTransform Knob { get; }
        void SetVisible(bool v);
    }
}