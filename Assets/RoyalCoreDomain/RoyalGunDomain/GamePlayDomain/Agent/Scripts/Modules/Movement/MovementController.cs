using System;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.MVC.Controller;
using RoyalCoreDomain.Scripts.Services.UpdateService;
using UnityEngine;

namespace RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Scripts.Modules.Movement
{
    public class MovementController : BaseController, IFixedUpdatable, IMovable
    {
        private readonly IMovementData _data;
        private readonly IMovementView _view;
        private Vector2 _moveVector;

        public MovementController(IMovementView view, IMovementData data)
        {
            _view = view;
            _data = data;
        }

        public void ManagedFixedUpdate(float dt)
        {
            Move();
        }

        public Vector2 Velocity { get; private set; }

        public void SetMoveVector(Vector2 moveVector)
        {
            _moveVector = Vector2.ClampMagnitude(moveVector, 1f);
        }

        public void Teleport(Vector2 p)
        {
            throw new NotImplementedException();
        }

        private void Move()
        {
            if (_view?.Rb == null)
            {
                Debug.LogError("[Movement] Rigidbody2D is null on view.");
                return;
            }

            var moveValue = _moveVector.normalized;
            Velocity = moveValue * _data.Speed;

            _view.Rb.linearVelocity = Velocity;
        }
    }
}