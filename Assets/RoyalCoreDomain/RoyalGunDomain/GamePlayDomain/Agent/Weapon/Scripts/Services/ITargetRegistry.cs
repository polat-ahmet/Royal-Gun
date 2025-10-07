using System.Collections.Generic;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Weapon.Scripts.Ports;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Services;

namespace RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Weapon.Scripts.Services
{
    public interface ITargetRegistry : IService
    {
        IReadOnlyCollection<ITargetable> All { get; }
        void Register(ITargetable t);
        void Unregister(ITargetable t);
    }
}