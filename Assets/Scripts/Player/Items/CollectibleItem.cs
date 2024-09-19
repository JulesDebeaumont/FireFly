using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Item on the ground / used by chest
public class CollectibleItem
{
    public int Count = 1;
    public Item Item;
    // public SoundEffect SoundEffect;

// TODO put this in Player.cs
    public void Collect()
    {
        PlayerManager.Instance.PlayerInventory.AddToInventory(Item, Count);
    }

}
