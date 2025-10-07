using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Weapon.Scripts.Ports;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Services;
using UnityEngine;

namespace RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Weapon.Scripts.Services
{
    public interface ITargetingService : IService
    {
        bool TryGetNearest(Vector2 origin, float range, out ITargetable best);
    }
}