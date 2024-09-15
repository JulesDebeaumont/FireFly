using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int GemCount = 0;
    public List<ItemInventory> Items = new List<ItemInventory>();

    private void Awake()
    {
        LoadInventory();
    }

    public void AddToInventory(Item Item, int Count)
    {
        // TODO if Items contient déjà le Item, on ++ au count, avec calcul de limit si ammo (exemple quiver 30, pasp lus de 30)
        ItemInventory itemToAdd = new ItemInventory {
            Item = Item,
            Count = Count
        };
        this.Items.Add(itemToAdd);
    }

    private void LoadInventory()
    {
        // TODO check in save file
    }

    public class ItemInventory
    {
        public Item Item;
        public int Count;
    }
}
