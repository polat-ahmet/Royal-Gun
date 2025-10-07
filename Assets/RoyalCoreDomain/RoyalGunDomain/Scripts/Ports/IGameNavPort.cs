using System.Threading;
using System.Threading.Tasks;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Scripts.Feature;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Context.Port;
using RoyalCoreDomain.Scripts.Framework.Template.RoyalFeatureTemplate.Scripts.Feature;

namespace RoyalCoreDomain.RoyalGunDomain.Scripts.Ports
{
    public interface IGameNavPort : IPort
    {
        Task OpenLobbyAsync();
        Task OpenGameplayAsync(GamePlayArgs args);
    }
}