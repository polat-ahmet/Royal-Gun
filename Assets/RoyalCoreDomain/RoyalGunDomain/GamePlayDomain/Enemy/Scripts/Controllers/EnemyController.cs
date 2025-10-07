using System;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Scripts.Modules.Health;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Scripts.Modules.Movement;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Scripts.Modules.SpriteRender;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Weapon.Scripts.Services;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Enemy.Scripts.Ports;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Player.Scripts.Models;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Player.Scripts.Views;
using RoyalCoreDomain.Scripts.Animation.Animator;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.MVC.Controller;
using RoyalCoreDomain.Scripts.Framework.Template.RoyalFeatureTemplate.Scripts.Models;
using RoyalCoreDomain.Scripts.Framework.Template.RoyalFeatureTemplate.Scripts.Services;
using RoyalCoreDomain.Scripts.Framework.Template.RoyalFeatureTemplate.Scripts.Views;
using RoyalCoreDomain.Scripts.Services.Audio;
using RoyalCoreDomain.Scripts.Services.UpdateService;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Enemy.Scripts.Controllers
{
    public class EnemyController : BaseController, IEnemyPort, IUpdatable
    {
        private readonly IAnimatable _animationController;
        private readonly IUpdateService<IFixedUpdatable> _fixedUpdateService;

        private readonly IHittable _healthController;
        private readonly AgentModel _model;

        private readonly IMovable _movementController;

        private readonly ITargetRegistry _registry;
        private readonly IRenderController _rendererController;

        private readonly Transform _target;
        private readonly AgentView _view;
        
        private readonly IAudioService _audioService;

        private Action OnEnemyDied{ get; set; }

        public EnemyController(AgentModel model, AgentView view, IAnimatable animationController,
            IHittable healthController, IMovable movementController, IRenderController rendererController,
            Transform target, ITargetRegistry registry, IUpdateService<IFixedUpdatable> fixedUpdateService, IAudioService audioService)
        {
            _model = model;
            _view = view;
            _animationController = animationController;
            _healthController = healthController;
            _movementController = movementController;
            _rendererController = rendererController;
            _target = target;
            _registry = registry;
            _fixedUpdateService = fixedUpdateService;
            _audioService = audioService;

            _view.BindTarget(this);

            _registry.Register(this);
            _fixedUpdateService.RegisterUpdatable(_movementController as IFixedUpdatable);
        }

        public float CurrentHealth => _healthController.CurrentHealth;
        public float MaxHealth => _healthController.MaxHealth;
        
        public void SetupCallbacks(Action onEnemyDied)
        {
            OnEnemyDied = onEnemyDied;
        }

        public void Hit(float amount)
        {
            _healthController.Hit(amount);
            if (!IsAlive) OnDie();
        }

        public Transform WeaponMount => _view.WeaponMount;
        public float DamageMultiplier => 1f;
        public Transform Transform => _view.transform;
        public bool IsAlive => _model.CurrentHealth > 0;

        public void ManagedUpdate(float dt)
        {
            if (!IsAlive || _target == null) return;

            Vector2 dir = (_target.position - _view.transform.position).normalized;

            _movementController.SetMoveVector(dir);
            // _animationController.PlayBool("Walk", dir.magnitude > 0);
            _rendererController.FaceDirection(dir);
        }

        private void OnDie()
        {
            Debug.Log("Enemy Controller Die");
            _audioService.PlayAudio(AudioClipType.EnemyDeath, AudioChannelType.Fx);
            OnEnemyDied?.Invoke();
        }

        public override void Dispose()
        {
            Debug.Log("Enemy Controller Dispose");
            OnEnemyDied = null;
            _fixedUpdateService.UnregisterUpdatable(_movementController as IFixedUpdatable);
            
            _registry.Unregister(this);
        }
    }
}