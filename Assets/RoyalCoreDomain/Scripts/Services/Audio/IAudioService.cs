using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Services;

namespace RoyalCoreDomain.Scripts.Services.Audio
{
    public interface IAudioService : IService
    {
        void InitEntryPoint();

        void PlayAudio(AudioClipType audioClipType, AudioChannelType audioChannel,
            AudioPlayType audioPlayType = AudioPlayType.OneShot);

        void StopAllAudio();
        void AddAudioClips(AudioClipsScriptableObject audioClipsScriptableObject);
        void RemoveAudioClips(AudioClipsScriptableObject audioClipsScriptableObject);
    }
}