using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Player.Scripts.Views;
using RoyalCoreDomain.Scripts.Animation.AnimationEvent;
using UnityEngine;

namespace RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Scripts.Modules.Animation
{
    public class AgentAnimationEventView : BaseAnimationEventView
    {
        [SerializeField] private AgentView agentView;

        private void Awake()
        {
            masterAnimationEventView = agentView;
        }
    }
}