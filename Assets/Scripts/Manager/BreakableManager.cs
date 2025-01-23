using System;
using System.Collections.Generic;
using System.Linq;
using Actors.Definitions;
using UnityEngine;

namespace Manager
{
    public class BreakableManager : MonoBehaviour
    {
        public static BreakableManager Instance { get; private set; }

        private void Awake()
        { 
            Instance = this;
        }
        
        public List<BreakableEntry> BreakableEntries { get; } = new();
        
        public void RegisterEntry(BreakableEntry breakableEntry)
        {
            BreakableEntries.Add(breakableEntry);
        }
        
        public void RemoveEntry(int breakableEntryInstanceId)
        {
            var entryToRemove = BreakableEntries.Single(entry => entry.InstanceID == breakableEntryInstanceId);
            BreakableEntries.Remove(entryToRemove);
        }
        
        public class BreakableEntry
        {
            public Collider Collider { get; }
            public Action<EDamageType> OnBreak { get; }
            public BreakableTable BreakableTable { get; }
            public int InstanceID { get; }

            public BreakableEntry(int instanceId, Collider collider, BreakableTable breakableTable, Action<EDamageType> onBreak)
            {
                InstanceID = instanceId;
                Collider = collider;
                BreakableTable = breakableTable;
                OnBreak = onBreak;
            }
                        
            public void TryBreak(EDamageType damageType)
            {
                if (!BreakableTable.CanBreak(damageType)) return;
                Instance.RemoveEntry(InstanceID);
                OnBreak(damageType);
            }
        }
    }
}