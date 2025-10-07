using RoyalCoreDomain.Scripts.Services.Pool;

namespace RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Weapon.Scripts.Bullet
{
    public class BulletFactoryService : IBulletFactory
    {
        private readonly IPool<BulletView> _pool;

        public BulletFactoryService(IPool<BulletView> pool)
        {
            _pool = pool;
        }

        public void SpawnBullet(in BulletSpawnInfo info)
        {
            // var bullet = _views.LoadView<BulletView>(info.BulletKey);
            var bullet = _pool.Rent();
            bullet.Fire(info, onHit: () => _pool.Return(bullet));
        }
    }
}