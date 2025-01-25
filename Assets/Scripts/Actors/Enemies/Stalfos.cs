using System;
using System.Collections.Generic;
using Actors.Ables;
using Actors.Definitions;
using Actors.Environments.CollectibleItems;
using Data.Definitions.CollectibleItems;
using UnityEngine;
using UnityEngine.Serialization;

namespace Actors.Enemies
{
    public class Stalfos : MonoBehaviour
    {
        private static readonly DamageTable DamageTable = new (new Dictionary<EDamageType, int>
            {
                { EDamageType.SWORD_REGULAR_SLASH , 3}
            }, 
            new Dictionary<EDamageState, int>
            {
                { EDamageState.STUNNED , 10 }
            });
        private static readonly DropTable DropTable = new (new Dictionary<Type, int>
        {
            { typeof(SmallAmber), 20 }
        }, EDropModifier.REGULAR);

        [FormerlySerializedAs("_damagableMonoHandler")] [SerializeField] private DamagableMonoHandler damagable;
        [SerializeField] private DropMonoHandler _dropMonoHandler;
        [SerializeField] private new Collider collider;
        
        private const int MaxHealth = 20;
        private int _currentHealth = MaxHealth;
        private bool _isDead = false;
        private bool _isInvicible = false;
        
        private void Awake()
        {
            damagable.Initialize(
                DamageTable,
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
            _dropMonoHandler.Initialize(DropTable);
        }

        private void Update()
        {
            
        }

        private void OnDamageTaken(int damageAmount, EDamageType damageType)
        {
            // TODO
        }

        private void OnDeath(EDamageType damageType)
        {
            _dropMonoHandler.PickAndSpawn(transform.position, EDropSpawnAnimation.HOP);
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

        private static int GetMaxHealth()
        {
            return MaxHealth;
        }
        
    }
}
