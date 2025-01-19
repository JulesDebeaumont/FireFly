using System;
using System.Collections.Generic;
using System.Linq;
using Actors.MonoHandlers;
using UnityEngine;

namespace Actors.Handlers
{
    public class BreakablHandler
    {
        public static readonly List<BreakableEntry> BreakabaleEntries = new();
        
        private static void RegisterEntry(BreakableEntry breakableEntry)
        {
            BreakabaleEntries.Add(breakableEntry);
        }
        
        private static void RemoveEntry(BreakableEntry breakableEntry)
        {
            BreakabaleEntries.Remove(breakableEntry);
        }
        
        private BreakableTable _breakableTable;
        private BreakableEntry _breakableEntry;
        private Action<DamageTable.EDamageType> _onBreak;
        private Action<bool> _setIsBreak;

        public BreakablHandler(
            BreakableTable breakableTable, 
            Collider colliderArg,
            Action<DamageTable.EDamageType> onBreak,
            Action<bool> setIsBreak
            )
        {
            _breakableTable = breakableTable;
            _breakableEntry = new BreakableEntry(this, colliderArg);
            _onBreak = onBreak;
            _setIsBreak = setIsBreak;
            AddSelfToEntries();
        }

        public void TryBreak(DamageTable.EDamageType damageType)
        {
            if (!_breakableTable.CanBreak(damageType)) return;
            RemoveSelfFromEntries();
            _setIsBreak(true);
            _onBreak(damageType);
        }

        public void RemoveSelfFromEntries()
        {
            RemoveEntry(_breakableEntry);
        }

        public void AddSelfToEntries()
        {
            RegisterEntry(_breakableEntry);
        }
        
        public class BreakableEntry
        {
            private BreakablHandler _breakablHandler;
            private Collider _collider;

            public BreakableEntry(BreakablHandler breakablHandler, Collider collider)
            {
                _breakablHandler = breakablHandler;
                _collider = collider;
            }
        }
    }

    public class BreakableTable
    {
        private DamageTable.EDamageType[] _breakableTable;

        public BreakableTable(DamageTable.EDamageType[] breakableTable)
        {
            _breakableTable = breakableTable;
        }

        public bool CanBreak(DamageTable.EDamageType damageType)
        {
            return _breakableTable.Any(breakDamageType => breakDamageType == damageType);
        }
    }
}
