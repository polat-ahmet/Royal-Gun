using RoyalCoreDomain.Scripts.Framework.RoyalFeature.MVC.Controller;
using UnityEngine;

namespace RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Scripts.Modules.Health
{
    public class HealthController : BaseController, IHittable
    {
        private readonly IHealthData _data;

        public HealthController(IHealthData data)
        {
            _data = data;
            _data.CurrentHealth = _data.MaxHealth;
        }

        public float CurrentHealth => _data.CurrentHealth;
        public float MaxHealth => _data.MaxHealth;

        public void Hit(float amount)
        {
            ApplyDamage(amount);
        }

        public void SetMax(float v)
        {
            _data.MaxHealth = v;
            if (_data.CurrentHealth > v) _data.CurrentHealth = v;
        }

        private void ApplyDamage(float dmg)
        {
            _data.CurrentHealth = Mathf.Max(0, _data.CurrentHealth - Mathf.Max(0, dmg));
        }

        public void Heal(float h)
        {
            _data.CurrentHealth = Mathf.Min(_data.MaxHealth, _data.CurrentHealth + Mathf.Max(0, h));
        }
    }
}