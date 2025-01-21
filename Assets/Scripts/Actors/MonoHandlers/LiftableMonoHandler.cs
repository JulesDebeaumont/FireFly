using System;
using UnityEngine;

namespace Actors.MonoHandlers
{
    // TODO maybe have a singleton since there can only be one actor being lifted by the player, same for the targetable mono handler
    public class LiftableMonoHandler : MonoBehaviour
    {
        private Action<bool> _setIsBeingLifted;
        private Func<bool> _getIsBeingLifted;
        
        public void Initialize(Action<bool> setIsBeingLifted, Func<bool> getIsBeingLifted)
        {
            _setIsBeingLifted = setIsBeingLifted;
            _getIsBeingLifted = getIsBeingLifted;
        }

        private void Update()
        {
            // TODO
        }
    }
}