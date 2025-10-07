using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Input.Scripts.Commands;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Input.Scripts.Models;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Input.Scripts.Ports;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Input.Scripts.Views;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Command;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.MVC.Controller;
using RoyalCoreDomain.Scripts.Framework.Template.RoyalFeatureTemplate.Scripts.Models;
using RoyalCoreDomain.Scripts.Framework.Template.RoyalFeatureTemplate.Scripts.Views;
using RoyalCoreDomain.Scripts.Services.UpdateService;
using UnityEngine;

namespace RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Input.Scripts.Controllers
{
    public class InputController : BaseController, IUpdatable, IJoystickInputPort
    {
        private readonly ICommandFactory _commandFactory;
        private readonly InputModel _model;
        private readonly IJoystickView _view;


        private Vector2 _anchor;

        public InputController(IJoystickView view, InputModel model, ICommandFactory commandFactory)
        {
            _view = view;
            _model = model;
            _commandFactory = commandFactory;
        }

        public Vector2 InputVector => _model.InputVector;
        public bool IsActive => _model.IsActive;

        public void ManagedUpdate(float dt)
        {
            PollMouse();

            if (_model.ShowOnTouch) _view.SetVisible(_model.IsActive);
        }

        private void PollMouse()
        {
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                _model.IsActive = true;
                _anchor = UnityEngine.Input.mousePosition;
                _view.Outline.position = _anchor;
                _view.Knob.position = _anchor;
                _model.InputVector = Vector2.zero;
            }

            else if (UnityEngine.Input.GetMouseButton(0) && _model.IsActive)
            {
                var pos = (Vector2)UnityEngine.Input.mousePosition;
                var delta = pos - _anchor;

                var dead = _model.DeadZone * _model.RadiusPx;
                var mag = Mathf.Max(0f, Mathf.Min(delta.magnitude, _model.RadiusPx) - dead);
                var dir = delta.sqrMagnitude > 0.0001f ? delta.normalized : Vector2.zero;

                var norm = mag <= 0f ? Vector2.zero : dir * (mag / (_model.RadiusPx - dead));
                _view.Knob.position = _anchor + dir * (mag + dead);
                _model.InputVector = Vector2.ClampMagnitude(norm, 1f);
            }

            else if (UnityEngine.Input.GetMouseButtonUp(0) && _model.IsActive)
            {
                _model.IsActive = false;
                _model.InputVector = Vector2.zero;
            }

            _commandFactory.CreateAndResolveCommand<InputChangedCommand>().SetData(new InputChangedCommandData(this))
                .Execute();
        }
    }
}