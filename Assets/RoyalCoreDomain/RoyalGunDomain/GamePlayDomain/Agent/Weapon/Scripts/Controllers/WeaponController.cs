using System.Collections;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Weapon.Scripts.Bullet;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Weapon.Scripts.Models;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Weapon.Scripts.Ports;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Weapon.Scripts.Services;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Weapon.Scripts.Views;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Weapon.Scripts.WeaponHolder;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Context;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.MVC.Controller;
using RoyalCoreDomain.Scripts.Framework.Template.RoyalFeatureTemplate.Scripts.Models;
using RoyalCoreDomain.Scripts.Framework.Template.RoyalFeatureTemplate.Scripts.Ports;
using RoyalCoreDomain.Scripts.Framework.Template.RoyalFeatureTemplate.Scripts.Services;
using RoyalCoreDomain.Scripts.Framework.Template.RoyalFeatureTemplate.Scripts.Views;
using RoyalCoreDomain.Scripts.Services.Audio;
using RoyalCoreDomain.Scripts.Services.Providers.ViewProvider;
using RoyalCoreDomain.Scripts.Services.UpdateService;
using UnityEngine;
using Random = System.Random;

namespace RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Weapon.Scripts.Controllers
{
    public class WeaponController : BaseController, IUpdatable, IWeaponPort
    {
        private readonly IAudioService _audio;
        private readonly FeatureContext _ctx;
        private readonly float _fireInterval; // 1 / FireRate
        private readonly WeaponModel _m;

        private readonly IBulletFactory _proj;
        private readonly ITargetRegistry _registry;
        private readonly Random _rng = new();

        private readonly ITargetingService _targeting;
        private readonly WeaponView _v;
        private readonly IViewProvider _views; // FX

        private IWeaponHolder _holder;
        private ITargetable _locked;

        public WeaponController(FeatureContext ctx, WeaponModel m, WeaponView v)
        {
            _ctx = ctx;
            _m = m;
            _v = v;
            _proj = _ctx.ImportService<IBulletFactory>();
            _audio = _ctx.TryImportService<IAudioService>(out var a) ? a : null;
            _views = _ctx.TryImportService<IViewProvider>(out var vp) ? vp : null;
            _targeting = ctx.ImportService<ITargetingService>();
            _registry = ctx.ImportService<ITargetRegistry>();

            _fireInterval = _m.FireRate <= 0f ? 0f : 1f / _m.FireRate;
        }

        public void ManagedUpdate(float dt)
        {
            if (_m.Cooldown > 0f) _m.Cooldown -= dt;
            if (_m.AmmoInMag == 0) Reload();
            if (_m.IsReloading) return;

            AutoAcquireAndAim(dt);

            if (_m.IsFiringHeld && CanFire)
                FireOnce();
        }

        public bool IsFiring => _m.IsFiringHeld;
        public bool IsReloading => _m.IsReloading;
        public int AmmoInMag => _m.AmmoInMag;
        public int MagSize => _m.MagSize;
        public bool CanFire => !_m.IsReloading && _m.Cooldown <= 0f && _m.AmmoInMag > 0;

        public void SetAim(Vector2 dir)
        {
            _m.AimDir = dir.sqrMagnitude > 1e-5f ? dir.normalized : _m.AimDir;
        }

        public void FireStart()
        {
            _m.IsFiringHeld = true;
        }

        public void FireStop()
        {
            _m.IsFiringHeld = false;
        }

        public void Reload()
        {
            if (_m.IsReloading || _m.AmmoInMag == _m.MagSize) return;
            _v.StartCoroutine(ReloadRoutine());
            _audio.PlayAudio(AudioClipType.Reload, AudioChannelType.Fx);
        }

        private void AutoAcquireAndAim(float dt)
        {
            bool NeedAcquire()
            {
                if (_locked == null || !_locked.IsAlive) return true;
                var dist2 = ((Vector2)_locked.Transform.position - (Vector2)_v.Muzzle.position).sqrMagnitude;
                return dist2 > _m.Range * _m.Range * 1.1f;
            }

            if (NeedAcquire())
            {
                var origin = (Vector2)_v.Muzzle.position;
                _targeting.TryGetNearest(origin, _m.Range, out _locked);
            }

            if (_locked != null)
            {
                var origin = (Vector2)_v.Muzzle.position;
                var tpos = (Vector2)_locked.Transform.position;

                Vector2 dir;
                dir = (tpos - origin).normalized;

                SetAim(dir);
                _m.IsFiringHeld = true;
                _v.LookAt(_m.AimDir);
            }
            else
            {
                _m.IsFiringHeld = false;
            }
        }

        public void AttachHolder(IWeaponHolder h)
        {
            _holder = h;
        }

        private IEnumerator ReloadRoutine()
        {
            _m.IsReloading = true;
            yield return new WaitForSeconds(_m.ReloadTime);
            _m.AmmoInMag = _m.MagSize;
            _m.IsReloading = false;
        }

        private void FireOnce()
        {
            _m.Cooldown = _fireInterval;
            _m.AmmoInMag--;

            var spread = _m.SpreadDegrees * Mathf.Deg2Rad;
            var r = ((float)_rng.NextDouble() * 2f - 1f) * spread;
            var dir = new Vector2(
                _m.AimDir.x * Mathf.Cos(r) - _m.AimDir.y * Mathf.Sin(r),
                _m.AimDir.x * Mathf.Sin(r) + _m.AimDir.y * Mathf.Cos(r)
            ).normalized;

            var pos = (Vector2)_v.Muzzle.position;
            var dmg = _m.BulletDamage * (_holder?.DamageMultiplier ?? 1f);

            _proj.SpawnBullet(new BulletSpawnInfo(
                pos, dir, _m.BulletSpeed, dmg, _m.BulletLife, _m.BulletKey
            ));
            
            _audio.PlayAudio(AudioClipType.Shoot, AudioChannelType.Fx);

            if (_views != null && !string.IsNullOrEmpty(_m.MuzzleFxKey))
            {
                // var fx = _views.LoadView<ParticleSystem>(_m.MuzzleFxKey);
                // fx.transform.position = pos;
                // fx.transform.rotation = Quaternion.FromToRotation(Vector3.right, dir);
                // fx.Play(true);
            }

            // if (_audio != null && !string.IsNullOrEmpty(_m.FireSfx))
            //     _audio.PlaySfx(new AudioKey(_m.FireSfx), new AudioParams{ Spatial=true, SpatialBlend=1f }, follow:_v.Muzzle);
        }
    }
}