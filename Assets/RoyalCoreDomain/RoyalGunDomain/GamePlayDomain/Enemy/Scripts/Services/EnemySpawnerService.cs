using System.Collections.Generic;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Weapon.Scripts.Services;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Enemy.Scripts.Feature;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Feature;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Services;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Services.FeatureFactory;
using RoyalCoreDomain.Scripts.Framework.Template.RoyalFeatureTemplate.Scripts.Services;
using RoyalCoreDomain.Scripts.Services.UpdateService;
using UnityEngine;

namespace RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Enemy.Scripts.Services
{
    public sealed class EnemySpawnerService : IService, IUpdatable
    {
        private readonly BaseFeature _baseFeature;
        private readonly IFeatureFactory _factory;
        private readonly float _interval;
        private readonly List<Vector2> _spawnPoints;
        private readonly ITargetRegistry _targetRegistry;
        private float _timer;

        public EnemySpawnerService(BaseFeature baseFeature, IFeatureFactory factory, ITargetRegistry targetRegistry,
            List<Vector2> spawns,
            float interval = 3f)
        {
            _baseFeature = baseFeature;
            _factory = factory;
            _targetRegistry = targetRegistry;
            _spawnPoints = spawns;
            _interval = interval;
        }

        public void ManagedUpdate(float dt)
        {
            _timer -= dt;
            if (_timer <= 0f)
            {
                _timer = _interval;
                SpawnEnemy();
            }
        }

        private void SpawnEnemy()
        {
            if (_spawnPoints.Count == 0) return;
            var spawnPoints = _spawnPoints[Random.Range(0, _spawnPoints.Count)];
            _factory.Create(_baseFeature, "Enemy" + _targetRegistry.All.Count,
                (addr, parent) => new EnemyFeature(addr, parent, spawnPoints)).Start();
        }
    }
}