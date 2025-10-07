using System;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Scripts.Modules.Health;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Scripts.Modules.Movement;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Scripts.Modules.SpriteRender;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Weapon.Scripts.WeaponHolder;
using RoyalCoreDomain.Scripts.Animation.AnimationEvent;
using RoyalCoreDomain.Scripts.Animation.Animator;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.MVC.View;
using RoyalCoreDomain.Scripts.Framework.Template.RoyalFeatureTemplate.Scripts.Views;
using UnityEngine;

namespace RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Player.Scripts.Views
{
    public sealed class AgentView : MonoBehaviour, IView, IMovementView, IAnimatorView, ISpriteRendererView,
        IAnimationEventView, IWeaponHolderView, IHittableViewBridge
    {
        private void Awake()
        {
            if (!Root) Root = transform;
            if (!Rb) Rb = GetComponent<Rigidbody2D>();
            if (!Animator) Animator = GetComponentInChildren<Animator>();
            if (!Renderer) Renderer = GetComponentInChildren<SpriteRenderer>();
        }

        public Action<AnimationEventPayloadSO> _OnAnimationEventTriggered { get; private set; }

        public void OnAnimationEvent(AnimationEventPayloadSO animationEvent)
        {
            _OnAnimationEventTriggered?.Invoke(animationEvent);
        }

        [field: SerializeField] public Animator Animator { get; private set; }

        public float CurrentHealth => Target.CurrentHealth;
        public float MaxHealth => Target.MaxHealth;

        public void Hit(float amount)
        {
            Target.Hit(amount);
        }

        public IHittable Target { get; private set; }

        public void BindTarget(IHittable target)
        {
            Target = target;
        }

        [field: SerializeField] public Transform Root { get; private set; }

        [field: SerializeField] public Rigidbody2D Rb { get; private set; }

        [field: SerializeField] public SpriteRenderer Renderer { get; private set; }

        [field: SerializeField] public Transform WeaponMount { get; private set; }

        public void SetupCallbacks(Action<AnimationEventPayloadSO> onAnimationEventTriggered)
        {
            _OnAnimationEventTriggered = onAnimationEventTriggered;
        }

        private void OnDestroy()
        {
            _OnAnimationEventTriggered = null;
        }
    }
}