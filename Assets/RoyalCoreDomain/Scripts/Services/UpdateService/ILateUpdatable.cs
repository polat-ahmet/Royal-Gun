namespace RoyalCoreDomain.Scripts.Services.UpdateService
{
    public interface ILateUpdatable : IUpdateKind
    {
        void ManagedLateUpdate(float dt);
    }
}