using UnityEngine;

namespace RoyalCoreDomain.Scripts.Animation.AnimationEvent
{
    [CreateAssetMenu(fileName = "AnimationEventPayload", menuName = "Royal Core/Animation Event/AnimationEventPayload",
        order = 0)]
    public class AnimationEventPayloadSO : ScriptableObject
    {
        public string eventName;
    }
}