using RoyalCoreDomain.Scripts.Framework.RoyalFeature.MVC.Model;
using UnityEngine;

namespace RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Input.Scripts.Models
{
    public class InputModel : BaseModel
    {
        public float ChangeThreshold;
        public float DeadZone;

        // Runtime state
        public Vector2 InputVector; // -1..1

        public bool IsActive;
        public float RadiusPx;
        public bool ShowOnTouch;


        public InputModel()
        {
        }

        public InputModel(InputCongfigSO so)
        {
            RadiusPx = so.RadiusPx;
            DeadZone = so.DeadZone;
            ChangeThreshold = so.ChangeThreshold;
            ShowOnTouch = so.ShowOnTouch;
        }
    }
}