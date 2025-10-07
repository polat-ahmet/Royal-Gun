using System;
using System.Collections.Generic;

namespace RoyalCoreDomain.Scripts.Services.UpdateService
{
    public class UpdateService<T> : IUpdateService<T> where T : IUpdateKind
    {
        private static readonly List<T> _updateObservers = new();

        private static int _currentUpdateIndex;

        public void Update(float deltaTime)
        {
            for (_currentUpdateIndex = _updateObservers.Count - 1; _currentUpdateIndex >= 0; _currentUpdateIndex--)
            {
                var observer = _updateObservers[_currentUpdateIndex];

                switch (observer)
                {
                    case IUpdatable updatable:
                        updatable.ManagedUpdate(deltaTime);
                        break;
                    case IFixedUpdatable fixedUpdatable:
                        fixedUpdatable.ManagedFixedUpdate(deltaTime);
                        break;
                    case ILateUpdatable lateUpdatable:
                        lateUpdatable.ManagedLateUpdate(deltaTime);
                        break;
                    default:
                        throw new Exception($"Unsupported observer type: {observer.GetType()}");
                }
            }
        }

        public void RegisterUpdatable(T observer)
        {
            var isCurrentlyIterating = _currentUpdateIndex > 0;
            if (isCurrentlyIterating)
            {
                _updateObservers.Insert(0, observer);
                _currentUpdateIndex++;
            }
            else
            {
                _updateObservers.Add(observer);
            }
        }

        public void UnregisterUpdatable(T observer)
        {
            var isCurrentlyIterating = _currentUpdateIndex > 0;
            if (isCurrentlyIterating)
            {
                var indexOfObserver = _updateObservers.IndexOf(observer);
                _updateObservers.Remove(observer);

                var wasObserverAlreadyIteratedThisFrame = indexOfObserver >= _currentUpdateIndex;
                if (!wasObserverAlreadyIteratedThisFrame) _currentUpdateIndex--;
            }
            else
            {
                _updateObservers.Remove(observer);
            }
        }
    }
}