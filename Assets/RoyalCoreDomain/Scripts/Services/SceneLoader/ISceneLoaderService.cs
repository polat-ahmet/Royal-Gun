using System.Threading;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Services;
using UnityEngine;

namespace RoyalCoreDomain.Scripts.Services.SceneLoader
{
    public interface ISceneLoaderService : IService
    {
        void InitEntryPoint();
        Awaitable<bool> TryLoadScene(string sceneName, CancellationTokenSource cancellationTokenSource);
        Awaitable<bool> TryUnloadScene(string sceneName, CancellationTokenSource cancellationTokenSource);
    }
}