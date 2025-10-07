using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RoyalCoreDomain.Scripts.Services.Providers.ModelProvider
{
    public class ResourcesModelProvider : IModelProvider
    {
        public T LoadSO<T>(string key, bool clone = false) where T : ScriptableObject
        {
            var k = NormalizeKey(key);
            var asset = Resources.Load<T>(k);
            if (asset == null)
                throw new InvalidOperationException(
                    $"[ModelProvider] SO not found at Resources/{k} (no extension; case-sensitive).");
            
            return clone ? Object.Instantiate(asset) : asset;
        }

        public void Release(Object obj)
        {
            if (obj != null) Object.Destroy(obj);
        }

        private static string NormalizeKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return key;
            var k = key.Trim();

            const string res = "Resources/";
            var idx = k.IndexOf(res, StringComparison.OrdinalIgnoreCase);
            if (idx >= 0) k = k[(idx + res.Length)..];

            if (k.EndsWith(".asset", StringComparison.OrdinalIgnoreCase))
                k = k[..^".asset".Length];

            if (k.StartsWith("/")) k = k[1..];
            return k;
        }
    }
}