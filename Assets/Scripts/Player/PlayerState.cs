#nullable enable

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public EPlayerState State = EPlayerState.STAND;
    public bool IsTargeting = false;
    public Actor? Target;

    public enum EPlayerState
    {
        STAND,
        WALK,
        RUN,
        SWIM,
        JUMP,
        TAKING_DAMAGE,
        ATTACK,
        CLIMB,
        SILENT,
        PROTECTING,
        LOOTING,
        TALKING,
        PUSHING
    }

    public enum ECollectAnimation
    {
        NONE,
        SMALL,
        MEDIUM,
        HIGH
    }
}
