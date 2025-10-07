using RoyalCoreDomain.Scripts.Framework.RoyalFeature.MVC.View;
using UnityEngine;

namespace RoyalCoreDomain.Scripts.Services.Audio
{
    public class AudioView : MonoBehaviour, IView
    {
        [SerializeField] private AudioSource _masterAudioSource;
        [SerializeField] private AudioSource _FxAudioSource;
        [SerializeField] private AudioSource _MusicAudioSource;

        public AudioSource MasterAudioSource => _masterAudioSource;
        public AudioSource FxAudioSource => _FxAudioSource;
        public AudioSource MusicAudioSource => _MusicAudioSource;
    }
}