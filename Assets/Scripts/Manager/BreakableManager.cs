using System.Collections.Generic;
using System.Linq;
using Actors.Composites;
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
        
        private List<BreakComposite> _breakableEntries = new();
        private List<BreakComposite> _breakableEntriesToDelete = new();
        
        public void RegisterEntry(BreakComposite breakable)
        {
            _breakableEntries.Add(breakable);
        }
        
        public void RemoveEntry(int instanceId)
        {
            var entryToRemove = _breakableEntries.Single(entry => entry.InstanceId == instanceId);
            _breakableEntries.Remove(entryToRemove);
        }

        public void TryBreakEntries(EDamageType damageType)
        {
            foreach (var breakableEntry in _breakableEntries)
            {
                // TODO check colliders
                if (!breakableEntry.BreakableTable.CanBreak(damageType)) return;
                breakableEntry.HasBreak = true;
                if (breakableEntry.HasBreak)
                {
                    _breakableEntriesToDelete.Add(breakableEntry);
                }
            }

            foreach (var breakableEntryToDelete in _breakableEntriesToDelete)
            {
                _breakableEntries.Remove(breakableEntryToDelete);
                breakableEntryToDelete.OnBreak(damageType);
            }
            _breakableEntriesToDelete.Clear();
        }
    }
}
