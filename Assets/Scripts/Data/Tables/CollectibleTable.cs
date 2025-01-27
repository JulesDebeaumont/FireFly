using System.Collections.Generic;
using Data.Definitions;
using UnityEngine;

namespace Data.Tables
{
    public static class CollectibleTable
    {
        private static readonly Dictionary<ECollectibleType, GameObject> Data = new()
        {
            { ECollectibleType.HEART, Resources.Load<GameObject>("Prefabs/Actors/Environments/CollectibleItems/Heart") },
            { ECollectibleType.SMALL_AMBER, Resources.Load<GameObject>("Prefabs/Actors/Environments/CollectibleItems/SmallAmber") },
        };

        public static GameObject Get(ECollectibleType collectibleType)
        {
            return Data[collectibleType];
        }
    }
}
