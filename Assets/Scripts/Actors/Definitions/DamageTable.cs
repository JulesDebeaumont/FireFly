using System.Collections.Generic;

namespace Actors.Definitions
{
    public class DamageTable
    {
        private Dictionary<EDamageType, int> _damageTable;
        private Dictionary<EDamageState, int> _damageMultiplier;
            
        public DamageTable(Dictionary<EDamageType, int> damageTypeTable,
            Dictionary<EDamageState, int> damageMultipliers)
        {
            _damageTable = damageTypeTable;
            _damageMultiplier = damageMultipliers;
        }

        public int GetDamageAmount(EDamageType damageType)
        {
            return _damageTable[damageType];
            // TODO apply modifiers
        }
    }
}