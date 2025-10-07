using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Services;

namespace RoyalCoreDomain.Scripts.Services.UpdateService
{
    public interface IUpdateService<T> : IService where T : IUpdateKind
    {
        void Update(float dt);
        void RegisterUpdatable(T updatable);
        void UnregisterUpdatable(T updatable);
    }
}