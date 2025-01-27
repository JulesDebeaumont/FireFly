using System;
using Actors.Definitions;
using Manager;
using UnityEngine;

namespace Actors.Composites
{
    public class BreakComposite
    {
        public bool HasBreak { get; set; } = false; 
        public Collider Collider { get; private set; }
        public BreakableTable BreakableTable { get; private set; }
        public int InstanceId { get; private set; }
        public Action<EDamageType> OnBreak { get; private set; }

        public BreakComposite(Collider collider, BreakableTable breakableTable, int instanceId)
        {
            Collider = collider;
            BreakableTable = breakableTable;
            InstanceId = instanceId;
        }

        public void RegisterToManager()
        {
            BreakableManager.Instance.RegisterEntry(this);
        }

        public void UnregisterFromManager()
        {
            BreakableManager.Instance.RemoveEntry(InstanceId);
        }
        
        public void TryBreakManual(EDamageType damageType)
        {
            if (!BreakableTable.CanBreak(damageType)) return;
            HasBreak = true;
            OnBreak(damageType);
        }
    }
}
