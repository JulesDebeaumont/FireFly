using Actors.Definitions;
using Manager;
using UnityEngine;

namespace Actors.Ables
{
    public interface IDamagable
    {
        Collider Collider { get; }
        int Health { get; set; }
        float InvincibilityDuration { get; set; }
        bool IsDead { get; set; }
        DamageTable DamageTable { get; set; }
        ETakeDamageVisualType TakeDamageVisualType { get; set; }
        void OnDamageTaken(EDamageType damageType);
        void OnDeath(EDamageType damageType);
        int GetInstanceId();

        public void RegisterToManager()
        {
            DamagableManager.Instance.RegisterEntry(this);
        }

        public void UnregisterFromManager()
        {
            DamagableManager.Instance.RemoveEntry(this);
        }

        private void TakeDamageManual(EDamageType damageType)
        {
            var damage = DamageTable.GetDamageAmount(damageType);
            if (damage == 0) return;
            Health -= damage;
            if (Health < 0) Health = 0;
            if (Health == 0)
            {
                IsDead = true;
                OnDeath(damageType);
            }
            else
            {
                OnDamageTaken(damageType);
            }
        }
    }
}
