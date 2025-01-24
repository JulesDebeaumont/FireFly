using System.Collections.Generic;
using System.Linq;
using Actors.Ables;
using UnityEngine;

namespace Manager
{
    public class DropableManager : MonoBehaviour
    {
        public static DropableManager Instance { get; private set; }

        private void Awake()
        { 
            Instance = this;
        }
        
        public List<IDropable> DroptableEntries = new ();
        
        public void RegisterEntry(IDropable dropable)
        {
            DroptableEntries.Add(dropable);
        }
        
        public void RemoveEntry(IDropable dropable)
        {
            var entry = DroptableEntries.Single(entry => entry.GetInstanceId() == dropable.GetInstanceId());
            DroptableEntries.Remove(entry);
        }
        
        private void Update()
        {
            DroptableEntries.ForEach(entry => entry.UpdateDropable());
        }
        
    }
}
