using System;
using System.Collections.Generic;
using System.Linq;
using Actors.Environments.CollectibleItems;
using Random = UnityEngine.Random;

namespace Actors.Definitions
{
    public class DropTable
    {
        private readonly Dictionary<Type, int> _data;
        private readonly EDropModifier _dropModifier;

        public DropTable(Dictionary<Type, int> data, EDropModifier dropModifier)
        {
            _data = data;
            _dropModifier = dropModifier;
        }

        public CollectibleItem Pick()
        {
            // apply modifier
            // apply player inventory restrictions (no bomb bag, then rupee chance += bomb chance)
            // if player full life -> more rupees
            // if player low life -> more hearts
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
}