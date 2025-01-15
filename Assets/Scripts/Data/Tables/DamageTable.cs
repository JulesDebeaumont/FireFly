using System.Collections.Generic;

namespace Data.Tables
{
    public class DamageTable // TODO refacto en faire en service pour les actors
    {
        public enum EDamageType
        {
            ICE_DAMAGE,
            FIRE_DAMAGE,
            SWORD_VERTICAL_SLASH,
            SWORD_HORIZONTAL_SLASH,
            JUMPSLASH,
            ARROW
        }

        public Dictionary<EDamageType, int> Data;
    }
}