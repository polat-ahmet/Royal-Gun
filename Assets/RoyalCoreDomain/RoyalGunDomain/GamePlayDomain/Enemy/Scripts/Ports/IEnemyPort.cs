using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Scripts.Modules.Health;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Weapon.Scripts.Ports;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Weapon.Scripts.WeaponHolder;
using RoyalCoreDomain.Scripts.Framework.Template.RoyalFeatureTemplate.Scripts.Ports;

namespace RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Enemy.Scripts.Ports
{
    public interface IEnemyPort : IHittable, IWeaponHolder, ITargetable
    {
    }
}