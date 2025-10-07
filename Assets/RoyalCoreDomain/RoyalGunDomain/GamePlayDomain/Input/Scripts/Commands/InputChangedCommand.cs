using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Scripts.Modules.Movement;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Scripts.Services.ControlledAgentService;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Command;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Context;

namespace RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Input.Scripts.Commands
{
    public class InputChangedCommand : BaseCommand, ICommandVoid
    {
        private InputChangedCommandData _inputChangedCommandData;
        private IMovable _inputControlledMovable;

        public void Execute()
        {
            _inputControlledMovable.SetMoveVector(_inputChangedCommandData._joystickInputPort.InputVector);
        }

        public override void Resolve(FeatureContext context)
        {
            var control = context.ImportService<IControlledAgentService>();
            if (control.TryGet<IMovable>(out var movable))
                _inputControlledMovable = movable;
        }

        public InputChangedCommand SetData(InputChangedCommandData inputChangedCommandData)
        {
            _inputChangedCommandData = inputChangedCommandData;
            return this;
        }
    }
}