using System.Collections.Generic;
using System.Linq;
using Actors.Ables;
using Actors.Definitions;
using UnityEngine;

namespace Manager
{
    public class BreakableManager : MonoBehaviour
    {
        public static BreakableManager Instance;

        private void Awake()
        { 
            Instance = this;
        }
        
        private List<BreakableEntry> _breakableEntries = new();
        private List<BreakableEntry> _breakableEntriesToDelete = new();
        
        public void RegisterEntry(IBreakable breakable)
        {
            _breakableEntries.Add(new BreakableEntry(breakable));
        }
        
        public void RemoveEntry(IBreakable breakable)
        {
            var entryToRemove = _breakableEntries.Single(entry => entry.Breakable.GetInstanceID() == breakable.GetInstanceID());
            _breakableEntries.Remove(entryToRemove);
        }

        public void TryBreakEntries(EDamageType damageType)
        {
            foreach (var breakableEntry in _breakableEntries)
            {
                // TODO check colliders
                if (!breakableEntry.Breakable.BreakableTable.CanBreak(damageType)) return;
                breakableEntry.Breakable.HasBreak = true;
                if (breakableEntry.Breakable.HasBreak)
                {
                    _breakableEntriesToDelete.Add(breakableEntry);
                }
            }

            foreach (var breakableEntryToDelete in _breakableEntriesToDelete)
            {
                _breakableEntries.Remove(breakableEntryToDelete);
                breakableEntryToDelete.Breakable.OnBreak(damageType);
            }
            _breakableEntriesToDelete.Clear();
        }

        private class BreakableEntry
        {
            public IBreakable Breakable;

            public BreakableEntry(IBreakable breakable)
            {
                Breakable = breakable;
            }
        }
    }
}
