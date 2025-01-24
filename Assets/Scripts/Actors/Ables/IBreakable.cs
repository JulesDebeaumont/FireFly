using Actors.Definitions;
using Manager;
using UnityEngine;

namespace Actors.Ables
{
    public interface IBreakable
    {
        bool HasBreak { set; }
        Collider Collider { get; }
        BreakableTable BreakableTable { get; }
        void OnBreak(EDamageType damageType);
        int GetInstanceID();

        public void Register()
        {
            BreakableManager.Instance.RegisterEntry(this);
        }

        public void Unregister()
        {
            BreakableManager.Instance.RemoveEntry(this);
        }
        
        public void TryBreak(EDamageType damageType)
        {
            if (!BreakableTable.CanBreak(damageType)) return;
            Unregister();
            HasBreak = true;
            OnBreak(damageType);
        }
    }
}
