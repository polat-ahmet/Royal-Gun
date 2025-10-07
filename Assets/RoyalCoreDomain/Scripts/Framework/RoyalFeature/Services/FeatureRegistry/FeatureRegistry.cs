using System;
using System.Collections.Generic;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Feature;

namespace RoyalCoreDomain.Scripts.Framework.RoyalFeature.Services.FeatureRegistry
{
    public sealed class FeatureRegistry : IFeatureRegistry
    {
        private readonly Dictionary<string, IFeature> _map = new(StringComparer.Ordinal);

        public void Register(IFeature f)
        {
            if (f == null) throw new ArgumentNullException(nameof(f));
            if (_map.ContainsKey(f.Address))
                throw new InvalidOperationException($"[FeatureRegistryService] Duplicate address: {f.Address}");
            _map[f.Address] = f;
        }

        public void Unregister(string address)
        {
            _map.Remove(address);
        }

        public bool TryGet(string address, out IFeature f)
        {
            return _map.TryGetValue(address, out f);
        }

        public IEnumerable<IFeature> All()
        {
            return _map.Values;
        }
    }
}