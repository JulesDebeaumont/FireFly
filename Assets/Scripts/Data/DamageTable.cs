using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTable
{
    public Dictionary<EDamageType, int> Data;

    public enum EDamageType
    {
        ICE_DAMAGE,
        FIRE_DAMAGE,
        SWORD_VERTICAL_SLASH,
        SWORD_HORIZONTAL_SLASH,
        JUMPSLASH,
        ARROW
    }
}
