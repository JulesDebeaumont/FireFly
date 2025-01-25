using System;
using System.Collections.Generic;
using System.Linq;
using Actors.Environments.CollectibleItems;
using Data.Definitions.CollectibleItems;
using Manager;
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

        public Type Pick()
        {
            var dataCopy = new Dictionary<Type, int>(_data);
            ApplyPlayerRestrictions(ref dataCopy);
            ApplyModifier(ref dataCopy);
            var nullChancePercentage = 100 - _data.Values.Sum();
            var randomValue = Random.Range(0, 100);
            if (randomValue < nullChancePercentage) return null;
            randomValue -= nullChancePercentage;
            foreach (var entry in dataCopy)
            {
                randomValue -= entry.Value;
                if (randomValue < 0) return entry.Key;
            }
            return null;
        }
        
        private void ApplyPlayerRestrictions(ref Dictionary<Type, int> data)
        {
            // TODO
            // no bomb bag -> NO Bomb Drop BUT + 10 rupees for example
            // etc...
        }

        private void ApplyModifier(ref Dictionary<Type, int> data)
        {
            switch (_dropModifier)
            {
                case EDropModifier.NONE:
                    return;
                    
                case EDropModifier.REGULAR:
                    if (data.ContainsKey(typeof(Heart)) && PlayerManager.Instance.player.playerInventory.IsLowHealth())
                    {
                        data[typeof(Heart)] += 30;
                    }
                    if (data.ContainsKey(typeof(SmallAmber)) && !PlayerManager.Instance.player.playerInventory.IsLowHealth())
                    {
                        data[typeof(SmallAmber)] += 30;
                    }
                    return;
            }
        }
    }
}