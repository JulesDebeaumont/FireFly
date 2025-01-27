using System;
using Actors.Definitions;
using Manager;
using UnityEngine;

namespace Actors.Composites
{
    public class DamageComposite
    {
        public int InstanceId { get; private set; }
        public Collider Collider { get; private set; }
        public int Health { get; set; }
        public int MaxHealth { get; private set; }
        public float InvincibilityDuration { get; private set; }
        public bool IsDead { get; set; } = false;
        public DamageTable DamageTable { get; private set; }
        public ETakeDamageVisualType TakeDamageVisualType { get; private set; }
        public Action<EDamageType> OnDamageTaken { get; private set; }
        public Action<EDamageType> OnDeath { get; private set; }
        
        public DamageComposite(int instanceId, Collider collider, int maxHealth, float invincibilityDuration, DamageTable damageTable, Action<EDamageType> onDamageTaken, Action<EDamageType> onDeath)
        {
            InstanceId = instanceId;
            Collider = collider;
            MaxHealth = maxHealth;
            InvincibilityDuration = invincibilityDuration;
            DamageTable = damageTable;
            OnDamageTaken = onDamageTaken;
            OnDeath = onDeath;
        }

        public void RegisterToManager()
        {
            DamagableManager.Instance.RegisterEntry(this);
        }

        public void UnregisterFromManager()
        {
            DamagableManager.Instance.RemoveEntry(InstanceId);
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
