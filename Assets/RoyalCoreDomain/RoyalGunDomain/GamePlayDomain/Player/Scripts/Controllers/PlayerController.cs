using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Scripts.Modules.Health;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Scripts.Modules.Movement;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Scripts.Modules.SpriteRender;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Player.Scripts.Commands;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Player.Scripts.Models;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Player.Scripts.Ports;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Player.Scripts.Views;
using RoyalCoreDomain.Scripts.Animation.AnimationEvent;
using RoyalCoreDomain.Scripts.Animation.Animator;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Command;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.MVC.Controller;
using RoyalCoreDomain.Scripts.Framework.Template.RoyalFeatureTemplate.Scripts.Models;
using RoyalCoreDomain.Scripts.Framework.Template.RoyalFeatureTemplate.Scripts.Views;
using RoyalCoreDomain.Scripts.Services.UpdateService;
using UnityEngine;

namespace RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Player.Scripts.Controllers
{
    public class PlayerController : BaseController, IPlayerPort
    {
        private readonly IAnimatable _animationController;

        private readonly ICommandFactory _commandFactory;

        private readonly IUpdateService<IFixedUpdatable> _fixedUpdateService;
        private readonly IHittable _healthController;
        private readonly AgentModel _model;

        private readonly IMovable _movementController;
        private readonly IRenderController _rendererController;
        private readonly AgentView _view;

        public PlayerController(AgentModel model, AgentView view, IAnimatable animationController,
            IHittable healthController, IMovable movementController, IRenderController rendererController,
            IUpdateService<IFixedUpdatable> fixedUpdateService, ICommandFactory commandFactory)
        {
            _model = model;
            _view = view;
            _animationController = animationController;
            _healthController = healthController;
            _movementController = movementController;
            _rendererController = rendererController;
            _fixedUpdateService = fixedUpdateService;
            _commandFactory = commandFactory;

            _view.BindTarget(this);
            _view.SetupCallbacks(OnAnimationEventTriggered);

            _fixedUpdateService.RegisterUpdatable(_movementController as IFixedUpdatable);
        }

        public float CurrentHealth => _healthController.CurrentHealth;
        public float MaxHealth => _healthController.MaxHealth;

        public void Hit(float amount)
        {
            _healthController.Hit(amount);
        }

        public Vector2 Velocity => _movementController.Velocity;

        public void SetMoveVector(Vector2 moveVector)
        {
            _movementController.SetMoveVector(moveVector);
            _animationController.PlayBool("Walk", moveVector.magnitude > 0);
            _rendererController.FaceDirection(moveVector);
        }

        public void Teleport(Vector2 p)
        {
            _movementController.Teleport(p);
        }

        public Transform WeaponMount => _view.WeaponMount;
        public float DamageMultiplier => 1f;

        public Transform transform => _view.transform;

        private void OnAnimationEventTriggered(AnimationEventPayloadSO animationEvent)
        {
            _commandFactory.CreateAndResolveCommand<FootStepAnimationEventCommand>().Execute();
        }

        public override void Dispose()
        {
            _fixedUpdateService.UnregisterUpdatable(_movementController as IFixedUpdatable);
        }
    }
}