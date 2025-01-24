using System.Collections.Generic;
using System.Linq;
using Actors.Ables;
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
        
        public List<IDamagable> DamagableEntries { get; } = new();
        
        public void RegisterEntry(IDamagable damagable)
        {
            DamagableEntries.Add(damagable);
        }
        
        public void RemoveEntry(IDamagable damagable)
        {
            var entry = DamagableEntries.Single(entry => entry.GetInstanceId() == damagable.GetInstanceId());
            DamagableEntries.Remove(entry);
        }
        
        private void Update()
        {
            DamagableEntries.ForEach(entry => entry.UpdateDamagable());
        }
    }
}
