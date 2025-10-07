using System;

namespace RoyalCoreDomain.Scripts.Animation.AnimationEvent
{
    public interface IAnimationEventView
    {
        public Action<AnimationEventPayloadSO> _OnAnimationEventTriggered { get; }
        void OnAnimationEvent(AnimationEventPayloadSO animationEvent);
    }
}