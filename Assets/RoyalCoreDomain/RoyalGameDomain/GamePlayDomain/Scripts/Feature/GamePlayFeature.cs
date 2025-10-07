using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Feature;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Services.FeatureFactory;

namespace RoyalCoreDomain.RoyalGameDomain.GamePlayDomain.Scripts.Feature
{
    public class GamePlayFeature : SceneOwnerFeature
    {
        protected override string SceneKey => "GamePlayScene";
        private readonly GamePlayArgs _args;
        
        private IFeatureFactory _factory;
        
        public GamePlayFeature(string address, IFeature parent, GamePlayArgs args) : base(address, parent)
        {
            _args = args;
        }

        protected override void OnPreInstall()
        {
        }

        protected override void OnInstall()
        {
        }
        
        // protected override async Task OnSceneReadyAsync(CancellationTokenSource ct)
        // {
        // }

        protected override void OnStart()
        {
  
        }

        protected override void OnDispose()
        {

        }
    }
}