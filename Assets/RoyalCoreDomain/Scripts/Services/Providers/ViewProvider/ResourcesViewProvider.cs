using System;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.MVC.View;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RoyalCoreDomain.Scripts.Services.Providers.ViewProvider
{
    public sealed class ResourcesViewProvider : IViewProvider
    {
        public T LoadView<T>(string key) where T : Component, IView
        {
            var k = NormalizeKey(key);
            
            var prefab = Resources.Load<GameObject>(k);

            if (prefab == null)
                throw new InvalidOperationException(
                    $"[ViewProvider] Prefab not found. Tried key: '{k}'" +
                    "Paths are case-sensitive and must be relative to any Resources/ folder, without extension.");
            
            var go = Object.Instantiate(prefab);
            
            var comp = go.GetComponentInChildren<T>(true);
            if (comp == null)
                throw new InvalidOperationException(
                    $"[ViewProvider] Instantiated '{prefab.name}' but no '{typeof(T).Name}' component found on root/children.");

            return comp;
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
            if (idx >= 0) k = k.Substring(idx + res.Length);
            
            if (k.EndsWith(".prefab", StringComparison.OrdinalIgnoreCase))
                k = k.Substring(0, k.Length - ".prefab".Length);
            
            if (k.StartsWith("/")) k = k.Substring(1);

            return k;
        }
    }
}