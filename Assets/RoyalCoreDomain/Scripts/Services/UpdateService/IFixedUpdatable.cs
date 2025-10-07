namespace RoyalCoreDomain.Scripts.Services.UpdateService
{
    public interface IFixedUpdatable : IUpdateKind
    {
        void ManagedFixedUpdate(float dt);
    }
}