using UnityEngine;

namespace RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Weapon.Scripts.WeaponHolder
{
    public interface IWeaponHolderView
    {
        Transform WeaponMount { get; }
    }
}