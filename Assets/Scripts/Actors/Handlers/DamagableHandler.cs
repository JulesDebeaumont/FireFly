using System;
using System.Collections.Generic;

namespace Actors.Handlers
{
    public class DamagableHandler
    {
        private DamageTable _damageTable;
        private bool _hasStartedDeathAnimation;
        private int _invincibilityTimer;
        
        public int MaxHealth;
        public int CurrentHealth;
        public bool IsDead;
        public bool IsInvincibile => _invincibilityTimer > 0;
        
        public event Action OnDeath;
        public event Action<int> OnHealthChanged;

        public DamagableHandler(DamageTable damageTable, int maxHealth, int invincibilityTimer)
        {
            _damageTable = damageTable;
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
            _invincibilityTimer = invincibilityTimer;
        }

        private void Die()
        {
            IsDead = true;
        }

        protected void TakeDamage(DamageTable.EDamageType damageType)
        {
            if (!DamageTable.Data.ContainsKey(damageType)) return;
            var damage = DamageTable.Data[damageType];
            damage *= multiplier;
            MaxHealth -= damage;
            if (MaxHealth < 0)
            {
                MaxHealth = 0;
            }

            if (MaxHealth == 0)
            {
                Die();
            }

            StartInvincibilityTimer();
        }

        protected void StartInvincibilityTimer()
        {
            // TODO
        }
        
        public class 

        public class DamageTable
        {
            public DamageTable(Dictionary<EDamageType, int> damageTypeTable,
                Dictionary<EDamageType, int> damageMultipliers)
            {
                
            }
            
            public int GetDamageByType
                    
            public enum EDamageType
            {
                ICE_DAMAGE,
                FIRE_DAMAGE,
                SWORD_VERTICAL_SLASH,
                SWORD_HORIZONTAL_SLASH,
                JUMPSLASH,
                ARROW
            }
        }
    }