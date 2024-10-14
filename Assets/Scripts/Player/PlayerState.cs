#nullable enable

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    private EPlayerState _state = EPlayerState.STAND;
    public void SetPlayerState(EPlayerState state)
    {
        _state = state;
    }

    public EPlayerState GetPlayerState()
    {
        return _state;
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

    public enum ECollectAnimation // TODO déplacer ailleur wesh
    {
        NONE,
        SMALL,
        MEDIUM,
        HIGH
    }
}
