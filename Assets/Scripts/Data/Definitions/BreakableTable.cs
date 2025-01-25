using System.Linq;

namespace Actors.Definitions
{

    public class BreakableTable
    {
        private EDamageType[] _breakableTable;

        public BreakableTable(EDamageType[] breakableTable)
        {
            _breakableTable = breakableTable;
        }

        public bool CanBreak(EDamageType damageType)
        {
            return _breakableTable.Any(breakDamageType => breakDamageType == damageType);
        }
    }
}
