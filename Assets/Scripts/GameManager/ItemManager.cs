using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public Item[] items;

    public Dictionary<string, Item> collectableItemsDict = new Dictionary<string, Item>();

    private void Awake()
    {
        foreach (Item collectable in items)
        {
            AddItem(collectable);
        }
    }

    private void AddItem(Item item)
    {

        if (item == null || item.data == null)
        {
            Debug.LogWarning($"Item is null: {item == null}, Item name: {(item != null ? item.name : "null")}");
        }


        if (!collectableItemsDict.ContainsKey(item.data.itemName))
        {
            collectableItemsDict.Add(item.data.itemName, item);
        }
    }

    public Item GetItemByName(string key)
    {
        if (collectableItemsDict.ContainsKey(key))
        {
            return collectableItemsDict[key];
        }

        return null;
    }
}
