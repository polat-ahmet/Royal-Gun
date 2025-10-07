using RoyalCoreDomain.Scripts.Framework.RoyalFeature.MVC.View;
using Unity.Cinemachine;
using UnityEngine;

namespace RoyalCoreDomain.Scripts.CameraFollow
{
    public sealed class CameraView : MonoBehaviour, IView
    {
        [field: SerializeField] public Camera Cam { get; private set; }

        [field: SerializeField] public CinemachineCamera vCam { get; private set; }
        // private void Awake() { Cam = GetComponent<Camera>(); }
    }
}