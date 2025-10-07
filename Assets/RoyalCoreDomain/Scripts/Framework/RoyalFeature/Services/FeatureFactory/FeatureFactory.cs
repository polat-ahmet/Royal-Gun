using System;
using System.Threading;
using System.Threading.Tasks;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Feature;
using UnityEngine;

namespace RoyalCoreDomain.Scripts.Framework.RoyalFeature.Services.FeatureFactory
{
    public sealed class FeatureFactory : IFeatureFactory
    {
        public T Create<T>(IFeature parent, string name, Func<string, IFeature, T> create, string id = null)
            where T : IFeature
        {
            if (create == null) throw new ArgumentNullException(nameof(create));
            var address = FeatureAddress.ChildOf(parent?.Address, name, id);
            var child = create(address, parent);

            BuildSync(child);
            
            return child;
        }
        
        public async Task<T> CreateAsync<T>(IFeature parent, string name, Func<string, IFeature, T> create, CancellationTokenSource ct,
            string id = null)
            where T : IFeature
        {
            Debug.Log("Feature Async Creating waiting");
            if (create == null) throw new ArgumentNullException(nameof(create));
            var address = FeatureAddress.ChildOf(parent?.Address, name, id);
            var child = create(address, parent);
            
            await BuildAsync(child, ct);
            
            return child;
        }
        
        public T CreateAndStart<T>(IFeature parent, string name, Func<string, IFeature, T> create, string id = null)
            where T : IFeature
        {
           var child = Create(parent, name, create, id);
           child.Start();
            return child;
        }
        
        public async Task<T> CreateAsyncAndStart<T>(IFeature parent, string name, Func<string, IFeature, T> create, CancellationTokenSource ct,
            string id = null)
            where T : IFeature
        {
            var child = await CreateAsync(parent, name, create, ct, id);
            child.Start();
            return child;
        }

        private void BuildSync(IFeature root)
        {
            root.PreInstall();
            root.Install();
            root.Resolve();
            // Warmup skipped → single frame setup
        }

        private async Task BuildAsync(IFeature root, CancellationTokenSource ct)
        {
            root.PreInstall();
            root.Install();
            root.Resolve();
            await root.WarmupAsync(ct);
        }

        public void Remove(IFeature feature)
        {
            if (feature == null) return;
            
            // lifecycle symmetry
            feature.Stop();
            feature.Parent?.RemoveChild(feature);
            feature.Dispose();
        }
    
    }
}