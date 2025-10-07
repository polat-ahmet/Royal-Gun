using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Services;
using Unity.Cinemachine;
using UnityEngine;

namespace RoyalCoreDomain.Scripts.CameraFollow
{
    public interface ICameraFollowService : IService
    {
        void BindCamera(Camera cam, CinemachineCamera vcam);
        void Follow(Transform target);
        void Unfollow();
    }
}