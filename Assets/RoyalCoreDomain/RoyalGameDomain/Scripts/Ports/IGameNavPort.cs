using System.Threading.Tasks;
using RoyalCoreDomain.RoyalGameDomain.GamePlayDomain.Scripts.Feature;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Context.Port;

namespace RoyalCoreDomain.RoyalGameDomain.Scripts.Ports
{
    public interface IGameNavPort : IPort
    {
        Task OpenLobbyAsync();
        Task OpenGameplayAsync(GamePlayArgs args);
    }
}