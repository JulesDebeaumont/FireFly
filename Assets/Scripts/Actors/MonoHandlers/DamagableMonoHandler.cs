using System;
using System.Collections.Generic;
using UnityEngine;

namespace Actors.MonoHandlers
{
    public class DamagableMonoHandler : MonoBehaviour
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
        
        private float _timestampInvicibilityStart; 
        private bool _isInvicibilityRunning;
        private DamageTable _damageTable;
        private DamagableEntry _damagableEntry;
        private ETakeDamageVisualType _takeDamageVisualType;
        private Action<int, DamageTable.EDamageType> _onDamageTaken;
        private Action<DamageTable.EDamageType> _onDeath;
        private Action<bool> _setIsDead;
        private Func<int> _getCurrentHealth;
        private Action<int> _setCurrentHealth;
        private Func<bool> _getIsInvincible;
        private Action<bool> _setIsInvincible;
        private Func<int> _getMaxHealth;
        private float _invicibilityDuration;

        public void Initialize(
            DamageTable damageTable, 
            Collider colliderArg,
            ETakeDamageVisualType takeDamageVisualType,
            Action<int, DamageTable.EDamageType> onDamageTaken,
            Action<DamageTable.EDamageType> onDeath,
            Action<bool> setIsDead,
            Func<int> getCurrentHealth,
            Action<int> setCurrentHealth,
            Func<bool> getIsInvincible,
            Action<bool> setIsInvincible,
            Func<int> getMaxHealth,
            float invicibilityDuration
            )
        {
            _damageTable = damageTable;
            _damagableEntry = new DamagableEntry(this, colliderArg);
            _takeDamageVisualType = takeDamageVisualType;
            _onDamageTaken = onDamageTaken;
            _onDeath = onDeath;
            _setIsDead = setIsDead;
            _getCurrentHealth = getCurrentHealth;
            _setCurrentHealth = setCurrentHealth;
            _getIsInvincible = getIsInvincible;
            _setIsInvincible = setIsInvincible;
            _getMaxHealth = getMaxHealth;
            _invicibilityDuration = invicibilityDuration;
            _isInvicibilityRunning = false;
            AddSelfToEntries();
        }

        public void Die()
        {
            _setIsDead(true);
            RemoveSelfFromEntries();
        }

        public void TakeDamage(DamageTable.EDamageType damageType)
        {
            var damage = _damageTable.GetDamage(damageType);
            var currentHealth = _getCurrentHealth();
            currentHealth -= damage;
            if (currentHealth < 0)
            {
                currentHealth = 0;
            }
            _setCurrentHealth(currentHealth);
            if (currentHealth == 0)
            {
                Die();
            }

            /* 
             * TODO apply damage state (ex: if damage type = ice, then spawn an ice block on the transform ONLY if weak to ice
             * Also, this application would be an Action<EDamageType> called right here
             */
            /*
             * TODO apply reaction to player, like electrocute if EDamageType is Sword, freeze player if ice keese etc..
             */
            StartInvincibilityTimer();
        }

        public void RemoveSelfFromEntries()
        {
            RemoveEntry(_damagableEntry);
        }

        public void AddSelfToEntries()
        {
            RegisterEntry(_damagableEntry);
        }

        private void Update()
        {
            if (_isInvicibilityRunning == false) return;
            switch (_takeDamageVisualType)
            {
                case ETakeDamageVisualType.FLASH_RED:
                    UpdateVisualFlashRed();
                    break;
                
                case ETakeDamageVisualType.PLAIN_RED:
                    UpdateVisualPlainRed();
                    break;
                
                case ETakeDamageVisualType.NONE:
                default: 
                    break;
            }
            var elapsed = Time.time - _timestampInvicibilityStart;
            if (elapsed < _invicibilityDuration) return;
            _isInvicibilityRunning = false;
        }

        private void UpdateVisualFlashRed()
        {
            // TODO
        }

        private void UpdateVisualPlainRed()
        {
            // TODO
        }

        private void StartInvincibilityTimer()
        {
            _isInvicibilityRunning = true;
            _timestampInvicibilityStart = Time.time;
        }

        public class DamagableEntry
        {
            private DamagableMonoHandler _damagableMonoHandler;
            private Collider _collider;

            public DamagableEntry(DamagableMonoHandler damagableMonoHandler, Collider collider)
            {
                _damagableMonoHandler = damagableMonoHandler;
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
            SWORD_REGULAR_SLASH,
            JUMPSLASH,
            ARROW,
            NEUTRAL
        }

        public enum EDamageState // TODO see later to have a StateHandler I guess
        {
            FROZEN,
            STUNNED,
            BURNING,
        }
    }
}