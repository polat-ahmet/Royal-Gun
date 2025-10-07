using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Services;

namespace RoyalCoreDomain.Scripts.Services.TimeService
{
    public interface ITimeService : IService
    {
        float DeltaTime { get; }
        float FixedDeltaTime { get; }
        float UnscaledDeltaTime { get; }
        float TimeScale { get; set; }
    }
}