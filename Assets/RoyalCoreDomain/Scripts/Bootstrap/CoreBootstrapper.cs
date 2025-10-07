using System;
using System.Threading;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Feature;
using UnityEngine;

namespace RoyalCoreDomain.Scripts.Bootstrap
{
    [AddComponentMenu("Game/Core/CoreBootstrapper")]
    [DefaultExecutionOrder(-20000)]
    public sealed class CoreBootstrapper : MonoBehaviour
    {
        private static bool _initialized;
        private IFeature _core;

        private void Awake()
        {
            if (_initialized)
            {
                Destroy(gameObject);
                return;
            }

            _initialized = true;
            DontDestroyOnLoad(gameObject);

            _core = new CoreFeature("Core", CancellationTokenSource.CreateLinkedTokenSource(Application.exitCancellationToken));
            _core.PreInstall();
            _core.Install();
            _core.Resolve();
        }

        private void Start()
        {
            _core.Start();
        }

        private void OnDestroy()
        {
            _core?.Dispose();
            _core = null;
            _initialized = false;
        }
    }
}