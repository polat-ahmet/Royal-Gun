using System;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.MVC.View;
using UnityEngine;
using UnityEngine.UI;

namespace RoyalCoreDomain.RoyalGameDomain.LobbyDomain.Scripts.View
{
    public class LobbyView : MonoBehaviour, IView
    {
        [SerializeField] Button startButton;

        private Action _onStartButtonPressed;

        public void SetupCallback(Action onStartButtonPressed)
        {
            _onStartButtonPressed = onStartButtonPressed;
        }
        
        private void Awake()
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
        }

        private void OnStartButtonClicked()
        {
            _onStartButtonPressed?.Invoke();
        }

        private void OnDestroy()
        {
            _onStartButtonPressed = null;
        }
    }
}