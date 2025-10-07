using System.Collections.Generic;
using System.Threading;
using RoyalCoreDomain.Scripts.Services.Logger;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RoyalCoreDomain.Scripts.Services.SceneLoader
{
    public class SceneLoaderService : ISceneLoaderService
    {
        private readonly HashSet<string> _loadedScenes = new ();
        private readonly HashSet<string> _loadingScenes = new ();
        
        public void InitEntryPoint()
        {
            AddOpenedScenesToLoadedHashset();
        }

        public async Awaitable<bool> TryLoadScene(string sceneName, CancellationTokenSource cancellationTokenSource)
        {
            var isSceneAlreadyLoaded = _loadedScenes.Contains(sceneName);

            if (isSceneAlreadyLoaded)
            {
                LogService.LogError($"scene:{sceneName} is already Loaded");
                return false;
            }

            var isSceneAlreadyLoading = _loadingScenes.Contains(sceneName);

            if (isSceneAlreadyLoading)
            {
                LogService.LogError($"scene:{sceneName} is already Loading");
                return false;
            }
            
            await LoadScene(sceneName, cancellationTokenSource);
            return true;
        }

        public async Awaitable<bool> TryUnloadScene(string sceneName, CancellationTokenSource cancellationTokenSource)
        {
            // var sceneName = sceneType.ToString();
            var isSceneAlreadyLoaded = _loadedScenes.Contains(sceneName);

            if (!isSceneAlreadyLoaded)
            {
                LogService.LogError($"scene:{sceneName} cant be unloaded as it is not Loaded");
                return false;
            }

            var isSceneAlreadyLoading = _loadingScenes.Contains(sceneName);

            if (isSceneAlreadyLoading)
            {
                LogService.LogError($"scene:{sceneName} cant be unloaded as it during Loading");
                return false;
            }

            await UnloadScene(sceneName, cancellationTokenSource);
            return true;
        }

        private void AddOpenedScenesToLoadedHashset()
        {
            var countLoaded = SceneManager.sceneCount;

            for (var i = 0; i < countLoaded; i++)
            {
                var sceneName = SceneManager.GetSceneAt(i).name;

                if (!_loadedScenes.Contains(sceneName))
                {
                    _loadedScenes.Add(sceneName);
                }
            }
        }

        private async Awaitable LoadScene(string sceneName, CancellationTokenSource cancellationTokenSource)
        {
            _loadingScenes.Add(sceneName);
            cancellationTokenSource.Token.ThrowIfCancellationRequested();
            await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            cancellationTokenSource.Token.ThrowIfCancellationRequested();
            _loadingScenes.Remove(sceneName);
            _loadedScenes.Add(sceneName);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        }

        private async Awaitable UnloadScene(string sceneName, CancellationTokenSource cancellationTokenSource)
        {
            // await _sceneInitiatorsService.InvokeInitiatorExitPoint(sceneType, cancellationTokenSource);
            // var sceneName = sceneType.ToString();
            await SceneManager.UnloadSceneAsync(sceneName);
            _loadedScenes.Remove(sceneName);
        }
    }
}