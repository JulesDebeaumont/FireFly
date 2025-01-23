using Actors.Definitions;
using Manager;
using UnityEngine;

namespace Actors.Ables
{
    public interface IDamagable
    {
        Collider Collider { get; }
        int Health { get; set; }
        int MaxHealth { get; set; }
        float InvicibilityDuration { get; set; }
        bool IsInvincible { get; set; }
        bool IsDead { get; set; }
        DamageTable DamageTable { get; set; }
        void OnDamageTaken(EDamageType damageType);
        void OnDeath(EDamageType damageType);
        int GetInstanceId();

        void Register()
        {
            DamagableManager.Instance.RegisterEntry(new DamagableManager.DamagableEntry(GetInstanceId(), Collider, ));
        }

        void Unregister()
        {
            DamagableManager.Instance.RemoveEntry(GetInstanceId());
        }
    }
    
    public class sdfsdf : MonoBehaviour
    {
        public void TakeDamage(EDamageType damageType)
        {
            var damage = _damageTable.GetDamageAmount(damageType);
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
             * Also, this application would be an Action<EDamageType> called right here ?
             */
            /*
             * TODO apply reaction to player, like electrocute if EDamageType is Sword, freeze player if ice keese etc..
             */
            StartInvincibilityTimer();
        }

        private void RemoveSelfFromEntries()
        {
            RemoveEntry(_damagableEntry);
        }

        private void AddSelfToEntries()
        {
            RegisterEntry(_damagableEntry);
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
        
    }
}