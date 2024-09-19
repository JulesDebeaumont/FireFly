#nullable enable

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Grass : EnvironmentActor
{
    public int Flag = 0;
    public bool HasBreak = false;
    public bool IsBeingLift = false;
    private static readonly DropTable _dropTable = new DropTable
    {
        Data = new Dictionary<Type, int>
        {
            { typeof(Heart), 20 },
            { typeof(Gem), 20 },
        }
    }; // TODO Dynamique selon les items déjà dans inventaire (bomb bag? full life ?)

    // Update is called once per frame
    void Update()
    {
        if (false && HasBreak == false)
        {
            Break();
        }
    }

    private void Break()
    {
        HasBreak = true;
        Item? ItemToSpawn = _dropTable.Pick();
        if (ItemToSpawn != null)
        {
            Instantiate(new Collectible
            {
                CollectibleItem = new CollectibleItem
                {
                    Item = ItemToSpawn
                },
                HopAtSpawn = true
            }, transform.position, Quaternion.Euler(0,0,0));
        }
        // anim
        // wait for anim to end
        Destroy(gameObject);
    }

}
