using System.Threading;
using System.Threading.Tasks;
using RoyalCoreDomain.Scripts.Services.SceneLoader;

namespace RoyalCoreDomain.Scripts.Framework.RoyalFeature.Feature
{
    public abstract class SceneOwnerFeature : BaseFeature
    {
        protected abstract string SceneKey { get; }

        private ISceneLoaderService _scenes;
        private CancellationTokenSource _ct;

        protected SceneOwnerFeature(string address, IFeature parent) : base(address, parent) { }

        protected override void OnResolve()
        {
            _scenes = Context.ImportService<ISceneLoaderService>();
        }

        protected override async Task OnWarmupAsync(CancellationTokenSource ct)
        {
            _ct = ct;
            
            await _scenes.TryLoadScene(SceneKey, ct);

            // Setup childs needs scene
            await OnSceneReadyAsync(ct);
        }

        /// Parent scene loaded → async setup childs scene dependent
        protected virtual Task OnSceneReadyAsync(CancellationTokenSource ct) => Task.CompletedTask;

        protected override void OnDispose()
        {
            // Dispose childs
            
            // Finally unload scene
            // fire-and-forget
            _ = _scenes.TryUnloadScene(SceneKey, default);
        }
    }
}