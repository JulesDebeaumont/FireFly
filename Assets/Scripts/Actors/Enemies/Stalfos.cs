using System.Collections.Generic;
using Actors.MonoHandlers;
using UnityEngine;

namespace Actors.Enemies
{
    public class Stalfos : MonoBehaviour
    {
        private static DamageTable _damageTable = new (new Dictionary<DamageTable.EDamageType, int>
            {
                { DamageTable.EDamageType.SWORD_REGULAR_SLASH , 3}
            }, 
            new Dictionary<DamageTable.EDamageState, int>
            {
                { DamageTable.EDamageState.STUNNED , 10 }
            });
        
        [SerializeField] private DamagableMonoHandler damagableMonoHandler;
        [SerializeField] private new Collider collider; // TODO check if new or not

        private const int MaxHealth = 20;
        private int _currentHealth = MaxHealth;
        private bool _isDead = false;
        private bool _isInvicible = false;
        
        private void Awake()
        {
            damagableMonoHandler.Initialize(
                _damageTable,
                collider,
                ETakeDamageVisualType.PLAIN_RED,
                OnDamageTaken,
                OnDeath,
                SetIsDead,
                GetCurrentHealth,
                SetCurrentHealth,
                GetIsInvincible,
                SetIsInvincible,
                GetMaxHealth,
                200
                );
        }

        private void Update()
        {
            
        }

        private void OnDamageTaken(int damageAmount, DamageTable.EDamageType damageType)
        {
            // TODO
        }

        private void OnDeath(DamageTable.EDamageType damageType)
        {
            // TODO
        }

        private void SetIsDead(bool isDead)
        {
            _isDead = isDead;
        }

        private int GetCurrentHealth()
        {
            return _currentHealth;
        }

        private void SetCurrentHealth(int currentHealth)
        {
            _currentHealth = currentHealth;
        }

        private bool GetIsInvincible()
        {
            return _isInvicible;
        }

        private void SetIsInvincible(bool isInvincible)
        {
            _isInvicible = isInvincible;
        }

        private int GetMaxHealth()
        {
            return MaxHealth;
        }
        
    }
}
