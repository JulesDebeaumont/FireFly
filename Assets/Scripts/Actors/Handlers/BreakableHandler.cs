using System;
using System.Collections.Generic;
using Actors.Definitions;
using UnityEngine;

namespace Actors.Handlers
{
    public class BreakableHandler
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
        private Action<EDamageType> _onBreak;
        private Action<bool> _setIsBreak;

        public BreakableHandler(
            BreakableTable breakableTable, 
            Collider colliderArg,
            Action<EDamageType> onBreak,
            Action<bool> setIsBreak
            )
        {
            _breakableTable = breakableTable;
            _breakableEntry = new BreakableEntry(this, colliderArg);
            _onBreak = onBreak;
            _setIsBreak = setIsBreak;
            AddSelfToEntries();
        }

        public void TryBreak(EDamageType damageType)
        {
            if (!_breakableTable.CanBreak(damageType)) return;
            RemoveSelfFromEntries();
            _setIsBreak(true);
            _onBreak(damageType);
        }
        
        private void RemoveSelfFromEntries()
        {
            RemoveEntry(_breakableEntry);
        }

        private void AddSelfToEntries()
        {
            RegisterEntry(_breakableEntry);
        }
        
        public class BreakableEntry
        {
            private BreakableHandler _breakableHandler;
            private Collider _collider;

            public BreakableEntry(BreakableHandler breakableHandler, Collider collider)
            {
                _breakableHandler = breakableHandler;
                _collider = collider;
            }
        }
    }
}
