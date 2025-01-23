using System;
using System.Collections.Generic;
using Actors.Definitions;
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
        
        private List<DropableEntry> _droptableEntries = new ();

        private void Update()
        {
            // TODO each in list...
        }

        public void AddToEntries(DropableEntry entry)
        {
            _droptableEntries.Add(entry);
        }

        private void RemoveFromEntries(DropableEntry entry)
        {
            _droptableEntries.Remove(entry);
        }
        
        private void PickAndSpawn(DropableEntry droptableEntry)
        {
            var collectibleType = droptableEntry.DropTable.Pick();
            if (collectibleType == null) return;
            SpawnCollectible(collectibleType, droptableEntry.Position, droptableEntry.Animation);
        }
        
        private void SpawnCollectible(Type collectibleType, Vector3 position, EDropSpawnAnimation animation)
        {
            switch (animation)
            {
                case EDropSpawnAnimation.STAND:
                    // TODO
                    break;
                
                case EDropSpawnAnimation.HOP:
                    // TODO
                    break;
                
                case EDropSpawnAnimation.BIG_HOP:
                    // TODO
                    break;
            }
        }
        
        public class DropableEntry
        {
            public Vector3 Position { get; }
            public EDropSpawnAnimation Animation { get; }
            public DropTable DropTable { get; }

            public DropableEntry(Vector3 position, EDropSpawnAnimation animation, DropTable dropTable)
            {
                Position = position;
                Animation = animation;
                DropTable = dropTable;
            }
        }
    }
}
