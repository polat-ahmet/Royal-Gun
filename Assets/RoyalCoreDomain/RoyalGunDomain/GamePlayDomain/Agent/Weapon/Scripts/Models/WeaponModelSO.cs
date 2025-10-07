using UnityEngine;

namespace RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Weapon.Scripts.Models
{
    [CreateAssetMenu(menuName = "Game/Weapon/WeaponModel")]
    public sealed class WeaponModelSO : ScriptableObject
    {
        [Header("Ballistics")] public float BulletSpeed = 18f;

        public float BulletDamage = 10f;
        public float BulletLife = 2.5f;
        public string BulletKey = "Bullets/StandardBullet"; // ViewProvider / Pool key
        public float Range = 12f;

        [Header("Firing")] public float FireRate = 8f; // rounds per second

        public int MagSize = 12;
        public float ReloadTime = 1.0f;
        public float SpreadDegrees = 2.5f; // random +/- (uniform)

        [Header("Fx/Sfx (opsiyonel)")] public string MuzzleFxKey = "FX/MuzzleFlash";

        public string FireSfx = "gun_shot";
        public string ReloadSfx = "reload";
    }
}