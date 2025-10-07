using UnityEngine;

namespace RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Input.Scripts.Views
{
    public class JoystickView : MonoBehaviour, IJoystickView
    {
        [SerializeField] private RectTransform outline;
        [SerializeField] private RectTransform knob;

        private void Awake()
        {
            SetVisible(false);
        }

        public RectTransform Outline => outline;
        public RectTransform Knob => knob;

        public void SetVisible(bool v)
        {
            gameObject.SetActive(v);
        }
    }
}