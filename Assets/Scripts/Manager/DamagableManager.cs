using System.Collections.Generic;
using System.Linq;
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
        
        public List<DamagableEntry> DamagableEntries { get; } = new();
        
        public void RegisterEntry(DamagableEntry damagableEntry)
        {
            DamagableEntries.Add(damagableEntry);
        }
        
        public void RemoveEntry(int damagableEntryId)
        {
            var entry = DamagableEntries.Single(entry => entry.InstanceId == damagableEntryId);
            DamagableEntries.Remove(entry);
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
        
        
        public class DamagableEntry
        {
            public int InstanceId { get; }
            public Collider Collider { get; }

            public DamagableEntry(int instanceId, Collider collider)
            {
                InstanceId = instanceId;
                Collider = collider;
            }
        }
    }
}
