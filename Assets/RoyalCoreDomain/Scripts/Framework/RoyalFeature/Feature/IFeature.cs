using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Context;

namespace RoyalCoreDomain.Scripts.Framework.RoyalFeature.Feature
{
    public interface IFeature : IDisposable
    {
        string Address { get; }
        IFeature Parent { get; }
        IReadOnlyList<IFeature> Children { get; }
        FeatureState State { get; }
        FeatureContext Context { get; }

        // lifecycle
        void PreInstall();
        void Install();
        Task WarmupAsync(CancellationTokenSource ct);
        void Resolve();
        void Start();
        void Pause();
        void Resume();
        void Stop();

        // tree
        void AddChild(IFeature child);
        void RemoveChild(IFeature child);
    }
}