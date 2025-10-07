using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Input.Scripts.Models;
using UnityEngine;

namespace RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Input.Scripts.Ports
{
    public class JoystickInputPortAdapter : IJoystickInputPort
    {
        private readonly InputModel _m;

        public JoystickInputPortAdapter(InputModel model)
        {
            _m = model;
        }

        public Vector2 InputVector => _m.InputVector;
        public bool IsActive => _m.IsActive;
    }
}