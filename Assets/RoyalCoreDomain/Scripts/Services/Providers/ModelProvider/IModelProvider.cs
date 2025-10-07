using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Services;
using UnityEngine;

namespace RoyalCoreDomain.Scripts.Services.Providers.ModelProvider
{
    public interface IModelProvider : IService
    {
        T LoadSO<T>(string key, bool clone = true) where T : ScriptableObject;
        void Release(Object obj);
    }
}