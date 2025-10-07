using Unity.Cinemachine;
using UnityEngine;

namespace RoyalCoreDomain.Scripts.CameraFollow
{
    public class CinemachineCameraService : ICameraFollowService
    {
        private Camera _cam;
        private CinemachineCamera _vcam;

        public void BindCamera(Camera cam, CinemachineCamera vcam)
        {
            _cam = cam;
            _vcam = vcam;
        }

        public void Follow(Transform target)
        {
            _vcam.Follow = target;
        }

        public void Unfollow()
        {
            if (_vcam != null) _vcam.Follow = null;
        }
    }
}