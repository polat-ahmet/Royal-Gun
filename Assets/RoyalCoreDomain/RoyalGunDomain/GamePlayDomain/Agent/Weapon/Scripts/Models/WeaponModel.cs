using RoyalCoreDomain.Scripts.Framework.RoyalFeature.MVC.Model;
using RoyalCoreDomain.Scripts.Framework.Template.RoyalFeatureTemplate.Scripts.Models;
using UnityEngine;

namespace RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Weapon.Scripts.Models
{
    public class WeaponModel : BaseModel
    {
        public Vector2 AimDir = Vector2.right;

        // state
        public int AmmoInMag;
        public string BulletKey;
        public float BulletSpeed, BulletDamage, BulletLife;
        public float Cooldown;
        public float FireRate;
        public bool IsFiringHeld;
        public bool IsReloading;
        public int MagSize;
        public string MuzzleFxKey, FireSfx, ReloadSfx;
        public float Range;
        public float ReloadTime;
        public float SpreadDegrees;

        public void CopyFrom(WeaponModelSO so)
        {
            BulletSpeed = so.BulletSpeed;
            BulletDamage = so.BulletDamage;
            BulletLife = so.BulletLife;
            BulletKey = so.BulletKey;
            FireRate = so.FireRate;
            MagSize = so.MagSize;
            ReloadTime = so.ReloadTime;
            SpreadDegrees = so.SpreadDegrees;
            MuzzleFxKey = so.MuzzleFxKey;
            FireSfx = so.FireSfx;
            ReloadSfx = so.ReloadSfx;
            AmmoInMag = MagSize;
            Range = so.Range;
        }
    }
}