﻿using System.Collections.Generic;
using UnityEngine;

public class InventoryInitializer : MonoBehaviour
{
    [Header("Items to add into inventory on start")]
    public List<InitialInventoryItem> itemsToAdd;

    public ItemManager itemManager;

    private const string InventoryInitKey = "InventoryInitialized";

    private void Start()
    {
        if (PlayerPrefs.GetInt(InventoryInitKey, 0) == 1)
        {
            Debug.Log("Inventory already initialized. Skipping starter items.");
            return;
        }

        if (GameManager.instance == null || GameManager.instance.player == null)
        {
            Debug.LogError("GameManager or Player is not ready. Cannot initialize inventory.");
            return;
        }

        var inventoryManager = GameManager.instance.player.inventoryManager;

        foreach (InitialInventoryItem entry in itemsToAdd)
        {
            if (entry.itemData != null && entry.quantity > 0)
            {
                
                var itemPrefab = itemManager.GetItemByName(entry.itemData.itemName);// Get the corresponding Item prefab instance according to itemData
                if (itemPrefab == null)
                {
                    Debug.LogWarning($"Item prefab not found for: {entry.itemData.itemName}");
                    continue;
                }

                for (int i = 0; i < entry.quantity; i++)
                {
                    Item itemInstance = Instantiate(itemPrefab);// Instantiate a new Item and add it to the Inventory
                    inventoryManager.Add("backpack", itemInstance);
                }
            }
        }

        GameManager.instance.uiManager.RefreshInventoryUI("backpack");

        PlayerPrefs.SetInt(InventoryInitKey, 1);
        PlayerPrefs.Save();
    }

}
