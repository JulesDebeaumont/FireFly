using System.Collections.Generic;
using System.Linq;
using Actors.Ables;
using Actors.Definitions;
using UnityEngine;

namespace Manager
{
    public class DamagableManager : MonoBehaviour
    {
        public static DamagableManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }
        
        private List<DamagableEntry> _damagableEntries = new();
        private List<DamagableEntry> _damagableEntriesToDelete = new();
        
        public void RegisterEntry(IDamagable damagable)
        {
            _damagableEntries.Add(new DamagableEntry(damagable));
        }
        
        public void RemoveEntry(IDamagable damagable)
        {
            var entry = _damagableEntries.Single(entry => entry.Damagable.GetInstanceId() == damagable.GetInstanceId());
            _damagableEntries.Remove(entry);
        }

        public void TryDamageEntries(EDamageType damageType)
        {
            foreach (var damagableEntry in _damagableEntries)
            {
                var damage = damagableEntry.Damagable.DamageTable.GetDamageAmount(damageType);
                if (damage == 0) continue;
                damagableEntry.Damagable.Health -= damage;
                if (damagableEntry.Damagable.Health < 0) damagableEntry.Damagable.Health = 0;
                if (damagableEntry.Damagable.Health == 0)
                {
                    damagableEntry.Damagable.IsDead = true;
                    _damagableEntriesToDelete.Add(damagableEntry);
                }
                else
                {
                    damagableEntry.HasTakenDamage = true;
                    damagableEntry.InvincibilityTimestamp = Time.time;
                    damagableEntry.Damagable.OnDamageTaken(damageType);
                }
            }

            foreach (var damagableEntryToDelete in _damagableEntriesToDelete)
            {
                _damagableEntries.Remove(damagableEntryToDelete);
                damagableEntryToDelete.Damagable.OnDeath(damageType);
            }
            _damagableEntriesToDelete.Clear();
        }
        
        private void Update()
        {
            foreach (var damagableEntry in _damagableEntries)
            {
                if (damagableEntry.HasTakenDamage == false) return;
                switch (damagableEntry.Damagable.TakeDamageVisualType)
                {
                    case ETakeDamageVisualType.NONE:
                        break;
                 
                    case ETakeDamageVisualType.FLASH_RED:
                        // TODO
                        break;

                    case ETakeDamageVisualType.PLAIN_RED:
                        // TODO
                        break;
                }
                var elapsed = Time.time - damagableEntry.InvincibilityTimestamp;
                if (elapsed < damagableEntry.Damagable.InvincibilityDuration) return;
                damagableEntry.HasTakenDamage = false;
            }
        }

        private class DamagableEntry
        {
            public readonly IDamagable Damagable;
            public float InvincibilityTimestamp;
            public bool HasTakenDamage;

            public DamagableEntry(IDamagable damagable)
            {
                Damagable = damagable;
            }
        }
    }
}
