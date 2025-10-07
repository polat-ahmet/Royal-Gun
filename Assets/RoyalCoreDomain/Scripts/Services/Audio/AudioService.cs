using System.Collections.Generic;
using UnityEngine;

namespace RoyalCoreDomain.Scripts.Services.Audio
{
    public class AudioService : IAudioService
    {
        private readonly List<AudioClipsScriptableObject> _audioClipsScriptableObjects = new();
        private readonly Dictionary<AudioChannelType, AudioSource> _audioSourceByChannel = new();
        private readonly AudioView _audioView;

        public AudioService(AudioView audioView)
        {
            _audioView = audioView;
            InitEntryPoint();
        }

        public void InitEntryPoint()
        {
            _audioSourceByChannel.Add(AudioChannelType.Master, _audioView.MasterAudioSource);
            _audioSourceByChannel.Add(AudioChannelType.Fx, _audioView.FxAudioSource);
            _audioSourceByChannel.Add(AudioChannelType.Music, _audioView.MusicAudioSource);
        }

        public void AddAudioClips(AudioClipsScriptableObject audioClipsScriptableObject)
        {
            _audioClipsScriptableObjects.Add(audioClipsScriptableObject);
        }

        public void RemoveAudioClips(AudioClipsScriptableObject audioClipsScriptableObject)
        {
            _audioClipsScriptableObjects.Remove(audioClipsScriptableObject);
        }

        public void PlayAudio(AudioClipType audioClipType, AudioChannelType audioChannel,
            AudioPlayType audioPlayType = AudioPlayType.OneShot)
        {
            TryPlayAudioClip(audioClipType, audioChannel, audioPlayType, out _);
        }

        public void StopAllAudio()
        {
            Debug.Log("Stop all audio");

            foreach (var keyValuePair in _audioSourceByChannel) keyValuePair.Value.Stop();
        }

        private bool TryPlayAudioClip(AudioClipType audioClipType, AudioChannelType audioChannel,
            AudioPlayType audioPlayType, out AudioClip audioClip)
        {
            audioClip = default;

            if (!TryGetAudioClip(audioClipType, out var clip)) return false;

            if (!_audioSourceByChannel.TryGetValue(audioChannel, out var audioSource))
            {
                Debug.LogError($"No audioChannel of name {audioChannel} found");
                return false;
            }

            var isAudioMuted = audioSource.mute || !audioSource.enabled;

            if (isAudioMuted) return false;

            switch (audioPlayType)
            {
                case AudioPlayType.OneShot:
                    audioSource.loop = false;
                    audioSource.PlayOneShot(clip);
                    break;
                case AudioPlayType.Loop:
                    audioSource.clip = clip;
                    audioSource.loop = true;
                    audioSource.Play();
                    break;
            }

            Debug.Log($"Played Audio {audioClipType} for channel {audioChannel}");
            return true;
        }

        private bool TryGetAudioClip(AudioClipType audioClipType, out AudioClip audioClip)
        {
            foreach (var audioClipsScriptableObject in _audioClipsScriptableObjects)
                if (audioClipsScriptableObject.AudioClips.TryGetValue(audioClipType, out audioClip))
                    return true;

            Debug.LogError($"No clip of name {audioClipType} found");
            audioClip = null;
            return false;
        }
    }
}