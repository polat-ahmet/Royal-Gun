namespace RoyalCoreDomain.Scripts.Services.UpdateService
{
    public interface IUpdatable : IUpdateKind
    {
        void ManagedUpdate(float dt);
    }
}