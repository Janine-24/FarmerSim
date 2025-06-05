using System.Collections.Generic;
using UnityEngine;

public class InventoryInitializer : MonoBehaviour
{
    [Header("Items to add into inventory on start")]
    public List<InitialInventoryItem> itemsToAdd;

    public ItemManager itemManager;

    private static bool inventoryHasBeenInitialized = false;
    private void Start()
    {
        if(inventoryHasBeenInitialized)
            return;

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
                // 根据 itemData 获取对应的 Item prefab 实例
                var itemPrefab = itemManager.GetItemByName(entry.itemData.itemName);
                if (itemPrefab == null)
                {
                    Debug.LogWarning($"Item prefab not found for: {entry.itemData.itemName}");
                    continue;
                }

                for (int i = 0; i < entry.quantity; i++)
                {
                    // 实例化一个新 Item 并添加进 Inventory
                    Item itemInstance = Instantiate(itemPrefab);
                    inventoryManager.Add("backpack", itemInstance);
                }
            }
        }

        GameManager.instance.uiManager.RefreshInventoryUI("backpack");
        inventoryHasBeenInitialized = true;    
    }

}
