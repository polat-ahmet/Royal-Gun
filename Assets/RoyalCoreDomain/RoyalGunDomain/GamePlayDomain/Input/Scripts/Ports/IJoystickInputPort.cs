using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Context.Port;
using UnityEngine;

namespace RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Input.Scripts.Ports
{
    public interface IJoystickInputPort : IPort
    {
        Vector2 InputVector { get; }
        bool IsActive { get; }
    }
}