using System.Collections.Generic;
using System.Linq;
using Actors.Ables;
using UnityEngine;

namespace Manager
{
    public class BreakableManager : MonoBehaviour
    {
        public static BreakableManager Instance { get; private set; }

        private void Awake()
        { 
            Instance = this;
        }
        
        public List<IBreakable> BreakableEntries { get; } = new();
        
        public void RegisterEntry(IBreakable breakable)
        {
            BreakableEntries.Add(breakable);
        }
        
        public void RemoveEntry(IBreakable breakable)
        {
            var entryToRemove = BreakableEntries.Single(entry => entry.GetInstanceID() == breakable.GetInstanceID());
            BreakableEntries.Remove(entryToRemove);
        }
    }
}