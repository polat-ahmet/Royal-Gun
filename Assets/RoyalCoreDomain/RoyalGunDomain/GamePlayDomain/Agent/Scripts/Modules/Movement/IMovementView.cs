using UnityEngine;

namespace RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Scripts.Modules.Movement
{
    public interface IMovementView
    {
        Transform Root { get; }
        Rigidbody2D Rb { get; }
    }
}