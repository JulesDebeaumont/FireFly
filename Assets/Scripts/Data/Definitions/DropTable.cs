using System.Collections.Generic;
using System.Linq;
using Actors.Definitions;
using Manager;
using Random = UnityEngine.Random;

namespace Data.Definitions
{
    public class DropTable
    {
        private readonly Dictionary<ECollectibleType, int> _data;
        private readonly EDropModifier _dropModifier;

        public DropTable(Dictionary<ECollectibleType, int> data, EDropModifier dropModifier)
        {
            _data = data;
            _dropModifier = dropModifier;
        }

        public ECollectibleType? Pick()
        {
            var dataCopy = new Dictionary<ECollectibleType, int>(_data);
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
        
        private void ApplyPlayerRestrictions(ref Dictionary<ECollectibleType, int> data)
        {
            // TODO
            // no bomb bag -> NO Bomb Drop BUT + 10 rupees for example
            // etc...
        }

        private void ApplyModifier(ref Dictionary<ECollectibleType, int> data)
        {
            switch (_dropModifier)
            {
                case EDropModifier.NONE:
                    return;
                    
                case EDropModifier.REGULAR:
                    if (data.ContainsKey(ECollectibleType.HEART) && PlayerManager.Instance.player.playerInventory.IsLowHealth())
                    {
                        data[ECollectibleType.HEART] += 30;
                    }
                    if (data.ContainsKey(ECollectibleType.SMALL_AMBER) && !PlayerManager.Instance.player.playerInventory.IsLowHealth())
                    {
                        data[ECollectibleType.SMALL_AMBER] += 30;
                    }
                    return;
            }
        }
    }
}