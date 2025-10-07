using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Context.Port;
using UnityEngine;

namespace RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Weapon.Scripts.WeaponHolder
{
    public interface IWeaponHolder : IPort
    {
        Transform WeaponMount { get; }
        float DamageMultiplier { get; }
    }
}