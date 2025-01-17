using System.Collections.Generic;
using UnityEngine;

namespace Actors.Handlers
{
    public class DamagableHandler
    {
        public static readonly List<DamagableEntry> DamagableEntries = new();
        
        private static void RegisterEntry(DamagableEntry damagableEntry)
        {
            DamagableEntries.Add(damagableEntry);
        }
        
        private static void RemoveEntry(DamagableEntry damagableEntry)
        {
            DamagableEntries.Remove(damagableEntry);
        }
        
        
        
        private DamageTable _damageTable;
        private bool _hasStartedDeathAnimation;
        private int _invincibilityTimer;
        private DamagableEntry _damagableEntry;
        private bool _isInvincibile = false;
        private ETakeDamageVisualType _takeDamageVisualType;

        private int _refMaxHealth;
        private int _refCurrentHealth; // TODO
        public bool IsDead;

        public DamagableHandler(DamageTable damageTable, Collider collider, ref int refMaxHealth, ref int invincibilityTimer, ETakeDamageVisualType takeDamageVisualType)
        {
            _damageTable = damageTable;
            _refMaxHealth = refMaxHealth;
            _refCurrentHealth = refMaxHealth;
            _invincibilityTimer = invincibilityTimer;
            _damagableEntry = new DamagableEntry(this, collider);
            _takeDamageVisualType = takeDamageVisualType;
            RegisterEntry(_damagableEntry);
        }

        private void Die()
        {
            IsDead = true;
            RemoveEntry(_damagableEntry);
        }

        public void TakeDamage(DamageTable.EDamageType damageType)
        {
            var damage = _damageTable.GetDamage(damageType);
            _refMaxHealth -= damage;
            if (_refMaxHealth < 0)
            {
                _refMaxHealth = 0;
            }

            if (_refMaxHealth == 0)
            {
                Die();
            }

            StartInvincibilityTimer();
        }
        
        public void StartInvincibilityTimer()
        {
            // TODO monobehaviour?
        }
        
        public void SetIsInvincible()
        {
            _isInvincibile = true;
            RemoveEntry(_damagableEntry);
        }

        public void UnsetIsInvincible()
        {
            _isInvincibile = false;
            RegisterEntry(_damagableEntry);
        }

        public bool IsInvincible()
        {
            return _isInvincibile;
        }

        public class DamagableEntry
        {
            private DamagableHandler _damagableHandler;
            private Collider _collider;

            public DamagableEntry(DamagableHandler damagableHandler, Collider collider)
            {
                _damagableHandler = damagableHandler;
                _collider = collider;
            }
        }
    }

    public enum ETakeDamageVisualType
    {
        NONE,
        FLASH_RED,
        PLAIN_RED
    }
    
    public class DamageTable
    {
        private Dictionary<EDamageType, int> _damageTable;
        private Dictionary<EDamageState, int> _damageMultiplier;
            
        public DamageTable(Dictionary<EDamageType, int> damageTypeTable,
            Dictionary<EDamageState, int> damageMultipliers)
        {
            _damageTable = damageTypeTable;
            _damageMultiplier = damageMultipliers;
        }

        public int GetDamage(EDamageType damageType)
        {
            return _damageTable[damageType];
            // TODO apply modifiers
        }
            
        public enum EDamageType
        {
            ICE_DAMAGE,
            FIRE_DAMAGE,
            SWORD_VERTICAL_SLASH,ETakeDamageVisualType
            SWORD_HORIZONTAL_SLASH,
            JUMPSLASH,
            ARROW
        }

        public enum EDamageState // TODO see later to have a StateHandler I guess
        {
            FROZEN,
            STUNNED,
            BURNING,
        }
    }
}