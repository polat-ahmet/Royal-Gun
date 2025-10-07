using UnityEngine;

namespace RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Weapon.Scripts.Bullet
{
    public readonly struct BulletSpawnInfo
    {
        public readonly Vector2 Position;
        public readonly Vector2 Direction; // normalize
        public readonly float Speed;
        public readonly float Damage;
        public readonly float LifeTime;
        public readonly string BulletKey; // prefab/resource id (pool key)

        public BulletSpawnInfo(Vector2 pos, Vector2 dir, float speed, float dmg, float life, string key)
        {
            Position = pos;
            Direction = dir;
            Speed = speed;
            Damage = dmg;
            LifeTime = life;
            BulletKey = key;
        }
    }
}