using System;
using System.Collections.Generic;
using Actors.Environments.CollectibleItems;
using Data.Definitions.CollectibleItems;
using UnityEngine;

namespace Data.Tables
{
    public static class CollectibleTable
    {
        private static readonly Dictionary<Type, GameObject> Data = new()
        {
            { typeof(Heart), Resources.Load<GameObject>("Prefabs/Actors/Environments/CollectibleItems/Heart") },
            { typeof(SmallAmber), Resources.Load<GameObject>("Prefabs/Actors/Environments/CollectibleItems/SmallAmber") },
        };

        public static GameObject Get(Type collectibleType)
        {
            return Data[collectibleType];
        }
    }
}
