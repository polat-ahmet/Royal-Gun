using System.Threading;
using System.Threading.Tasks;
using RoyalCoreDomain.RoyalGameDomain.GamePlayDomain.Scripts.Feature;
using RoyalCoreDomain.RoyalGameDomain.LobbyDomain.Scripts.View;
using RoyalCoreDomain.RoyalGameDomain.Scripts.Ports;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Feature;
using RoyalCoreDomain.Scripts.Services.UI;

namespace RoyalCoreDomain.RoyalGameDomain.LobbyDomain.Scripts.Feature
{
    public class LobbyFeature : SceneOwnerFeature
    {
        private LobbyView _view;
        private bool _navigating;
        protected override string SceneKey => "LobbyScene";
        
        
        public LobbyFeature(string address, IFeature parent) : base(address, parent)
        {
        }
 
        protected override Task OnSceneReadyAsync(CancellationTokenSource ct)
        {
            _view = Context.ImportService<IUIService>().Show<LobbyView>("Lobby/LobbyView");
            return Task.CompletedTask;
        }

        protected override void OnStart()
        {
            _view.SetupCallback(OnPlayClicked);
        }

        private void OnPlayClicked()
        {
            if (_navigating) return; 
            _navigating = true;
            
            var nav = Context.ImportPort<IGameNavPort>();
            _ = nav.OpenGameplayAsync(new GamePlayArgs{ LevelIndex = 1});
        }

        protected override void OnDispose()
        {
            if (Context.TryImportService<IUIService>(out var uiService) && _view != null) uiService.Close(_view);
            _view = null;
            base.OnDispose();
        }
    }
}