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
            BreakableManager.Instance.RegisterEntry(new BreakableManager.BreakableEntry(GetInstanceID() ,Collider, BreakableTable, OnBreak));
        }

        public void Unregister()
        {
            BreakableManager.Instance.RemoveEntry(GetInstanceID());
        }
    }
}
