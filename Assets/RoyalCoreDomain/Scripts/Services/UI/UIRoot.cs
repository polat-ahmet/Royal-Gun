using RoyalCoreDomain.Scripts.Framework.RoyalFeature.MVC.View;
using UnityEngine;
using UnityEngine.UI;

namespace RoyalCoreDomain.Scripts.Services.UI
{
    public sealed class UIRoot : MonoBehaviour, IUIRoot, IView
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private CanvasScaler scaler;
        [SerializeField] private GraphicRaycaster raycaster;

        [SerializeField] private RectTransform hudLayer;
        [SerializeField] private RectTransform overlayLayer;
        [SerializeField] private RectTransform popupLayer;
        [SerializeField] private RectTransform systemLayer;

        public Canvas Canvas => canvas;

        public Transform GetLayerRoot(UILayer layer)
        {
            return layer switch
            {
                UILayer.HUD => hudLayer,
                UILayer.Overlay => overlayLayer,
                UILayer.Popup => popupLayer,
                UILayer.System => systemLayer,
                _ => hudLayer
            };
        }
    }
}