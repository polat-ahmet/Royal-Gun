using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Command;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Context;
using RoyalCoreDomain.Scripts.Services.Audio;

namespace RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Player.Scripts.Commands
{
    public class FootStepAnimationEventCommand : BaseCommand, ICommandVoid
    {
        private IAudioService _audioService;

        public void Execute()
        {
            _audioService.PlayAudio(AudioClipType.Walk, AudioChannelType.Fx);
        }

        public override void Resolve(FeatureContext context)
        {
            _audioService = context.ImportService<IAudioService>();
        }
    }
}