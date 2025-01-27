using System.Collections.Generic;
using Actors.Definitions;
using Data.Definitions;
using UnityEngine;

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
        private static readonly DropTable DropTable = new (new Dictionary<ECollectibleType, int>
        {
            { ECollectibleType.SMALL_AMBER, 20 }
        }, EDropModifier.REGULAR);

        [SerializeField] private new Collider collider;
        
        private const int MaxHealth = 20;
        private int _currentHealth = MaxHealth;
        private bool _isDead = false;
        private bool _isInvicible = false;
        
        private void Awake()
        {
            
        }

        private void Update()
        {
            
        }
    }
}
