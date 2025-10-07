using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Context.Port;
using UnityEngine;

namespace RoyalCoreDomain.Scripts.Services.UI
{
    public interface IUIRoot : IPort
    {
        Transform GetLayerRoot(UILayer layer);
    }
}