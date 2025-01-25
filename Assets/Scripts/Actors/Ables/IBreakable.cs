using Actors.Definitions;
using Manager;
using UnityEngine;

namespace Actors.Ables
{
    public interface IBreakable
    {
        bool HasBreak { get; set; }
        Collider Collider { get; }
        BreakableTable BreakableTable { get; }
        void OnBreak(EDamageType damageType);
        int GetInstanceID();

        public void RegisterToManager()
        {
            BreakableManager.Instance.RegisterEntry(this);
        }

        public void UnregisterFromManager()
        {
            BreakableManager.Instance.RemoveEntry(this);
        }
        
        public void TryBreakManual(EDamageType damageType)
        {
            if (!BreakableTable.CanBreak(damageType)) return;
            HasBreak = true;
            OnBreak(damageType);
        }
    }
}
