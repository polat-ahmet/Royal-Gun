using RoyalCoreDomain.Scripts.Utilities.Helpers.SerializableDictionary;
using UnityEngine;

namespace RoyalCoreDomain.Scripts.Services.Audio
{
    public abstract class AudioClipsScriptableObject : ScriptableObject
    {
        public SerializableDictionary<AudioClipType, AudioClip> AudioClips;
    }
}