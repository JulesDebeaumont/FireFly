using System;
using System.Collections.Generic;
using System.Linq;
using Actors.Environments.CollectibleItems;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Actors.Handlers
{
    public class DropCollectibleHandler
    {
        private readonly DropTable _dropTable;
        private readonly EDropModifier _dropModifier;
        
        public DropCollectibleHandler(Dictionary<Type, int> dropData, EDropModifier modifier)
        {
            _dropTable = new DropTable(dropData);
            _dropModifier = modifier;
        }
        
        public void PickAndSpawn(Vector3 position, ESpawnAnimation animation)
        {
            ApplyModifier();
            if (!EnsureDropIsAllowed()) return;
            SpawnCollectible();
        }

        private void ApplyModifier()
        {
            // TODO
        }

        private bool EnsureDropIsAllowed()
        {
            // TODO
            return true;
        }

        private void SpawnCollectible()
        {
            // TODO
            // switch ESpawnAnimation
        }
        
        private class DropTable // TODO make as definiotns, same for enum
        {
            private readonly Dictionary<Type, int> _data;

            public DropTable(Dictionary<Type, int> data)
            {
                _data = data;
            }

            public CollectibleItem Pick()
            {
                // apply dropModifier
                var nullChancePercentage = 100 - _data.Values.Sum();
                var randomValue = Random.Range(0, 100);
                if (randomValue < nullChancePercentage) return null;
                randomValue -= nullChancePercentage;
                foreach (var entry in _data)
                {
                    randomValue -= entry.Value;
                    if (randomValue < 0) return Activator.CreateInstance(entry.Key) as CollectibleItem;
                }
                return null;
            }
        }
        
        public enum ESpawnAnimation
        {
            STAND,
            HOP,
            BIG_HOP
        }

        public enum EDropModifier
        {
            NONE, // no change to the table
            REGULAR // low life -> more hearts, full life -> more ambers
        }
    }
}