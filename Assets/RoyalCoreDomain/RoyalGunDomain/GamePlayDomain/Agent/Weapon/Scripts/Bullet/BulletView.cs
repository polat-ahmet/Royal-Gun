using System;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Scripts.Modules.Health;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.MVC.View;
using RoyalCoreDomain.Scripts.Services.Pool;
using UnityEngine;

namespace RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Weapon.Scripts.Bullet
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public sealed class BulletView : MonoBehaviour, IView, IPoolable
    {
        private float _damage;
        private Rigidbody2D _rb;
        private float _ttl;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }
        
        private Action _onReturn;

        private void Update()
        {
            _ttl -= Time.deltaTime;
            if (!(_ttl <= 0f)) return;
            
            if(_onReturn == null)
                Return();
            else
                _onReturn.Invoke();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent<IHittable>(out var h)) return;
            
            h.Hit(_damage);
            if(_onReturn == null)
                Return();
            else
                _onReturn.Invoke();
        }

        public void Fire(BulletSpawnInfo info, Action onHit)
        {
            transform.position = info.Position;
            var vel = info.Direction * info.Speed;
            _rb.linearVelocity = vel;
            _damage = info.Damage;
            _ttl = info.LifeTime;
            _onReturn = onHit;
        }

        private void Return()
        {
            Destroy(gameObject);
        }

        public void OnRent()
        {
            Debug.Log("Bullet Rent");
        }

        public void OnReturn()
        {
            Debug.Log("Bullet Return");
        }
    }
}