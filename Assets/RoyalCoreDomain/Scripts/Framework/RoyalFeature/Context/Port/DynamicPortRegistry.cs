using System;
using System.Collections.Generic;
using System.Linq;

namespace RoyalCoreDomain.Scripts.Framework.RoyalFeature.Context.Port
{
    public sealed class DynamicPortRegistry : IPortRegistry
    {
        private readonly Dictionary<string, (object port, PortMeta meta)> _byId = new(StringComparer.Ordinal);
        private readonly Dictionary<Type, HashSet<string>> _byType = new();

        public string Register<TPort>(TPort port, PortMeta meta = null) where TPort : class, IPort
        {
            if (port == null) throw new ArgumentNullException(nameof(port));
            var id = meta?.Id;
            if (string.IsNullOrWhiteSpace(id)) id = $"{typeof(TPort).Name}:{Guid.NewGuid():N}";

            if (_byId.ContainsKey(id))
                throw new InvalidOperationException($"[PortRegistry] Duplicate id: {id}");

            _byId[id] = (port, meta ?? new PortMeta { Id = id, Tags = Array.Empty<string>() });

            if (!_byType.TryGetValue(typeof(TPort), out var set))
                _byType[typeof(TPort)] = set = new HashSet<string>();
            set.Add(id);

            return id;
        }

        public bool Unregister(string id)
        {
            if (!_byId.TryGetValue(id, out var entry)) return false;
            var type = entry.port.GetType().GetInterfaces().FirstOrDefault(i => typeof(IPort).IsAssignableFrom(i)) ??
                       entry.port.GetType();
            if (_byType.TryGetValue(type, out var set)) set.Remove(id);
            return _byId.Remove(id);
        }

        public bool TryResolve<TPort>(out TPort port, Func<PortMeta, bool> predicate = null) where TPort : class, IPort
        {
            port = null;
            if (!_byType.TryGetValue(typeof(TPort), out var set) || set.Count == 0) return false;

            foreach (var id in set)
            {
                var (p, meta) = _byId[id];
                if (p is TPort cast && (predicate == null || predicate(meta)))
                {
                    port = cast;
                    return true;
                }
            }

            return false;
        }

        public IReadOnlyList<(TPort port, PortMeta meta)> ResolveMany<TPort>(Func<PortMeta, bool> predicate = null)
            where TPort : class, IPort
        {
            if (!_byType.TryGetValue(typeof(TPort), out var set) || set.Count == 0)
                return Array.Empty<(TPort, PortMeta)>();

            var list = new List<(TPort, PortMeta)>(set.Count);
            foreach (var id in set)
            {
                var (p, meta) = _byId[id];
                if (p is TPort cast && (predicate == null || predicate(meta)))
                    list.Add((cast, meta));
            }

            return list;
        }
    }
}