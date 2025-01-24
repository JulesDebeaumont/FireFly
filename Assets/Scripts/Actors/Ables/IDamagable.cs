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
        float InvincibilityTimeStamp { get; set; }
        bool HasTakenDamage { get; set; }
        bool IsDead { get; set; }
        bool IsRegsiteredAsDamagable { get; set; }
        DamageTable DamageTable { get; set; }
        ETakeDamageVisualType TakeDamageVisualType { get; set; }
        void OnDamageTaken(EDamageType damageType);
        void OnDeath(EDamageType damageType);
        int GetInstanceId();

        private void Register()
        {
            DamagableManager.Instance.RegisterEntry(this);
            IsRegsiteredAsDamagable = true;            
        }

        private void Unregister()
        {
            DamagableManager.Instance.RemoveEntry(this);
            IsRegsiteredAsDamagable = false;
        }

         void TryTakeDamage(EDamageType damageType)
         { 
            if (!IsRegsiteredAsDamagable) Register();
            var damage = DamageTable.GetDamageAmount(damageType);
            if (damage == 0) return;
            Health -= damage;
            if (Health < 0) Health = 0;
            if (Health == 0)
            {
                IsDead = true;
                Unregister();
                OnDeath(damageType);
            }
            else
            {
                HasTakenDamage = true;
                InvincibilityTimeStamp = Time.time;
                OnDamageTaken(damageType);
            }
        }

         void UpdateDamagable()
         {
             if (HasTakenDamage == false) return;
             switch (TakeDamageVisualType)
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
             var elapsed = Time.time - InvincibilityTimeStamp;
             if (elapsed < InvincibilityDuration) return;
             HasTakenDamage = false;
         }
    }
}
