using UnityEngine;

namespace RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Input.Scripts.Models
{
    [CreateAssetMenu(fileName = "InputModel", menuName = "Game/Config/InputModel", order = 0)]
    public class InputCongfigSO : ScriptableObject
    {
        [Header("Joystick")] public float RadiusPx = 100f;

        [Range(0f, 0.5f)] public float DeadZone = 0.1f;

        [Range(0f, 0.2f)]
        public float ChangeThreshold = 0.02f;

        [Header("Visual")] public bool ShowOnTouch = true;
    }
}