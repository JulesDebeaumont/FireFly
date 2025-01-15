#nullable enable

using UnityEngine;

namespace Player
{
    public class PlayerState : MonoBehaviour
    {
        public enum ECollectAnimation // TODO déplacer ailleur wesh
        {
            NONE,
            SMALL,
            MEDIUM,
            HIGH
        }

        public enum EPlayerState
        {
            STAND,
            WALK,
            RUN,
            SWIM,
            JUMP,
            KNOCKED_BACK,
            ATTACK,
            CLIMB,
            PROTECTING,
            LOOTING,
            TALKING,
            PUSHING,
            ROLLING,
            LOOKING
        }

        private EPlayerState _state = EPlayerState.STAND;

        public void SetPlayerState(EPlayerState state)
        {
            _state = state;
        }

        public EPlayerState GetPlayerState()
        {
            return _state;
        }
    }
}