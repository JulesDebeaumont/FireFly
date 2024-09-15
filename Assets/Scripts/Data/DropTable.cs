#nullable enable

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class DropTable
{
    public Dictionary<Type, int> Data = new Dictionary<Type, int> { };

    public Item? Pick()
    {
        int nullChancePercentage = 100 - this.Data.Values.Sum();
        int randomValue = UnityEngine.Random.Range(0, 100);
        if (randomValue < nullChancePercentage)
        {
            return null;
        }
        randomValue -= nullChancePercentage;
        foreach (var entry in this.Data)
        {
            randomValue -= entry.Value;
            if (randomValue < 0)
            {
                return EnsureTypeIsItem(entry.Key);
            }
        }

        return null;
    }

    private Item EnsureTypeIsItem(Type type)
    {
        if (!type.IsAssignableFrom(typeof(Item)))
        {
            throw new Exception("Droptable dictionary data has a key which is not an item !!");
        }
        return Activator.CreateInstance(type) as Item;
    }
}
