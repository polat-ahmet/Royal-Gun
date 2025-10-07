using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Services;

namespace RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Weapon.Scripts.Bullet
{
    public interface IBulletFactory : IService
    {
        void SpawnBullet(in BulletSpawnInfo info);
    }
}