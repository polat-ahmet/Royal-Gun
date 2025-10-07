using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Context.Port;
using UnityEngine;

namespace RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Scripts.Modules.Movement
{
    public interface IMovable : IPort
    {
        Vector2 Velocity { get; }
        void SetMoveVector(Vector2 moveVector);
        void Teleport(Vector2 p);
    }
}