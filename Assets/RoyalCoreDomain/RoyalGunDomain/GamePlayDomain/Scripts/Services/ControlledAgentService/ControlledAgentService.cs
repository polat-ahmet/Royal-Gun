using System;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Player.Scripts.Ports;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Context.Port;
using UnityEngine;

namespace RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Scripts.Services.ControlledAgentService
{
    public sealed class ControlledAgentService : IControlledAgentService
    {
        private IPlayerPort _current;

        public Transform TryGetTransform()
        {
            if (_current == null)
                throw new InvalidOperationException("Controlled agent service has not been started yet.");

            return _current.transform;
        }

        public void Set(IPlayerPort m)
        {
            _current = m;
        }

        public bool TryGet<T>(out T m) where T : class, IPort
        {
            m = _current as T;
            return m != null;
        }
    }
}