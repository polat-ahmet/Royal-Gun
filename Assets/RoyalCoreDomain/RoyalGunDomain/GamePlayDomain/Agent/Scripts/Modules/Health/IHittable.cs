using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Context.Port;

namespace RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Scripts.Modules.Health
{
    public interface IHittable : IPort
    {
        float CurrentHealth { get; }
        float MaxHealth { get; }

        void Hit(float amount);
    }
}