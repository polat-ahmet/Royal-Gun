using System.Collections.Generic;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Weapon.Scripts.Ports;

namespace RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Weapon.Scripts.Services
{
    public sealed class TargetRegistry : ITargetRegistry
    {
        private readonly HashSet<ITargetable> _set = new();
        public IReadOnlyCollection<ITargetable> All => _set;

        public void Register(ITargetable t)
        {
            if (t != null) _set.Add(t);
        }

        public void Unregister(ITargetable t)
        {
            if (t != null) _set.Remove(t);
        }
    }
}