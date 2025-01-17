using System.Collections.Generic;
using Actors.Handlers;
using UnityEngine;

namespace Actors.Enemies
{
    public class Stalfos : MonoBehaviour
    {
        private static DamageTable _damageTable = new (new Dictionary<DamageTable.EDamageType, int>
            {
                { DamageTable.EDamageType.SWORD_VERTICAL_SLASH , 3}
            }, 
            new Dictionary<DamageTable.EDamageState, int>
            {
                { DamageTable.EDamageState.STUNNED , 10 }
            });
        private DamagableHandler _damagableHandler;
        private int _maxHealth = 20;
        private int _invicibilityTimer = 200;
        public new Collider collider; // TODO check if new or not
        
        private void Awake()
        {
            _damagableHandler = new DamagableHandler(_damageTable, collider, ref _maxHealth, ref _invicibilityTimer, true);
        }

        private void Update()
        {
            
        }
    }
}
