using RoyalCoreDomain.Scripts.Services.TimeService;
using UnityEngine;

namespace RoyalCoreDomain.Scripts.Services.UpdateService
{
    [AddComponentMenu("Game/Core/TickDriver")]
    [DefaultExecutionOrder(-5000)]
    public sealed class UpdateDriver : MonoBehaviour
    {
        private IUpdateService<IFixedUpdatable> _fixedUpdateService;
        private IUpdateService<ILateUpdatable> _lateUpdateService;
        private ITimeService _timeService;
        private IUpdateService<IUpdatable> _updateService;

        private void Update()
        {
            if (_updateService == null || _timeService == null) return;
            _updateService.Update(_timeService.DeltaTime);
        }

        private void FixedUpdate()
        {
            if (_fixedUpdateService == null || _timeService == null) return;
            _fixedUpdateService.Update(_timeService.FixedDeltaTime);
        }

        private void LateUpdate()
        {
            if (_lateUpdateService == null || _timeService == null) return;
            _lateUpdateService.Update(_timeService.DeltaTime);
        }

        public void Initialize(ITimeService timeService, IUpdateService<IUpdatable> updateService,
            IUpdateService<IFixedUpdatable> fixedUpdateService, IUpdateService<ILateUpdatable> lateUpdateService)
        {
            _timeService = timeService;
            _updateService = updateService;
            _fixedUpdateService = fixedUpdateService;
            _lateUpdateService = lateUpdateService;
        }
    }
}