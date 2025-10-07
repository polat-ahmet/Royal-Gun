using RoyalCoreDomain.Scripts.Framework.RoyalFeature.MVC.View;
using UnityEngine;

namespace RoyalCoreDomain.Scripts.Bootstrap
{
    [DisallowMultipleComponent]
    public sealed class CoreView : MonoBehaviour, IView
    {
        [Header("Anchors")] public Transform Root; // optional

        public Transform DriversParent; // Add here runtime Mono's like TickDriver

        private void Reset()
        {
            if (Root == null) Root = transform;
            if (DriversParent == null) DriversParent = transform;
        }
    }
}