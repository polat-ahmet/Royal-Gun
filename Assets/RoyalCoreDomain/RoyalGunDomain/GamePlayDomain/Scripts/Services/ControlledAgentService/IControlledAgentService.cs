using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Player.Scripts.Ports;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Context.Port;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Services;
using UnityEngine;

namespace RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Scripts.Services.ControlledAgentService
{
    public interface IControlledAgentService : IService
    {
        public Transform TryGetTransform();
        void Set(IPlayerPort movable);
        public bool TryGet<T>(out T m) where T : class, IPort;
    }
}