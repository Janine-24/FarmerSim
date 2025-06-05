using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Inventory;

public class Inventory_UI : MonoBehaviour
{
    public string inventoryName;
    public List<Slot_UI> slots = new List<Slot_UI>();
    public Canvas canvas;


    private Inventory inventory;

    private void Start()
    {
        if (GameManager.instance == null || GameManager.instance.player == null)
        {
            Debug.LogError("GameManager or Player is not ready. Cannot initialize inventory.");
            return;
        }

        canvas = FindFirstObjectByType<Canvas>();
        inventory = GameManager.instance.player.inventoryManager.GetInventoryByName(inventoryName);

        if (inventory == null)
        {
            Debug.LogError("Could not find inventory with name: " + inventoryName);
            return;
        }

        SetupSlots();
        LoadInventoryData();
        Refresh();
    }

    public void Refresh()
    {

        if (inventory == null)
        {
            Debug.LogWarning("Inventory not assigned for UI: " + inventoryName);
            return;
        }

        if (inventory.slots == null)
        {
            Debug.LogError("Inventory slots list is null!");
            return;
        }

        if (slots.Count == inventory.slots.Count)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].slotID < 0)
                {
                    slots[i].slotID = i;
                }

                if (inventory.slots[i].itemName != "")
                {
                    slots[i].SetItem(inventory.slots[i]);
                }
                else
                {
                    slots[i].SetEmpty();
                }
            }
        }
        else
        {
            Debug.LogWarning("Slot count mismatch between UI and Inventory.");
        }
    }

    public void Remove()
    {
        if (UI_Manager.draggedSlot != null)
        {
            Item itemToDrop = GameManager.instance.itemManager.GetItemByName(inventory.slots[UI_Manager.draggedSlot.slotID].itemName);

            if (itemToDrop != null)
            {
                if (UI_Manager.dragSingle)
                {
                    GameManager.instance.player.DropItem(itemToDrop);
                    inventory.Remove(UI_Manager.draggedSlot.slotID);
                }
                else
                {
                    GameManager.instance.player.DropItem(itemToDrop, inventory.slots[UI_Manager.draggedSlot.slotID].count);
                    inventory.Remove(UI_Manager.draggedSlot.slotID, inventory.slots[UI_Manager.draggedSlot.slotID].count);
                }

                Refresh();
            }
        }

        UI_Manager.draggedSlot = null;
    }

    public void SlotBeginDrag(Slot_UI slot)
    {
        UI_Manager.draggedSlot = slot;

        UI_Manager.draggedIcon = Instantiate(UI_Manager.draggedSlot.itemIcon);
        UI_Manager.draggedIcon.transform.SetParent(canvas.transform);
        UI_Manager.draggedIcon.raycastTarget = false;
        UI_Manager.draggedIcon.rectTransform.sizeDelta = new Vector2(50, 50);

        MoveToMousePosition(UI_Manager.draggedIcon.gameObject);
    }

    public void SlotDrag()
    {
        if (UI_Manager.draggedSlot != null)
        {
            MoveToMousePosition(UI_Manager.draggedIcon.gameObject);
        }
    }

    public void SlotEndDrag()
    {
        Destroy(UI_Manager.draggedIcon.gameObject);
        UI_Manager.draggedIcon = null;
    }


    public void SlotDrop(Slot_UI slot)
    {
        if (UI_Manager.dragSingle)
        {
            UI_Manager.draggedSlot.inventory.MoveSlot(
                UI_Manager.draggedSlot.slotID,
                slot.slotID,
                slot.inventory);
        }
        else
        {
            UI_Manager.draggedSlot.inventory.MoveSlot(
                UI_Manager.draggedSlot.slotID,
                slot.slotID,
                slot.inventory,
                UI_Manager.draggedSlot.inventory.slots[UI_Manager.draggedSlot.slotID].count);
        }

        // 保存被拖出的 inventory
        var sourceUI = UI_Manager.draggedSlot.GetComponentInParent<Inventory_UI>();
        if (sourceUI != null)
        {
            sourceUI.SaveInventoryData();
        }

        // 保存被放入的 inventory
        var targetUI = slot.GetComponentInParent<Inventory_UI>();
        if (targetUI != null)
        {
            targetUI.SaveInventoryData();
        }

        GameManager.instance.uiManager.RefreshAll();
    }


    private void MoveToMousePosition(GameObject toMove)
    {
        if (canvas != null)
        {
            Vector2 position;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, null, out position);

            toMove.transform.position = canvas.transform.TransformPoint(position);
        }
    }

    private void SetupSlots()
    {
        int counter = 0;

        foreach (Slot_UI slot in slots)
        {
            if (slot == null)
            {
                Debug.LogWarning("A Slot_UI is null in the slots list.");
                continue;
            }
            Debug.Log($"UI Slots Count: {slots.Count}, Inventory Slots Count: {inventory.slots.Count}");


            slot.slotID = counter;
            slot.inventory = inventory;
            counter++;
        }
    }

    public void SaveInventoryData()
    {
        if (inventory == null) return;

        for (int i = 0; i < inventory.slots.Count; i++)
        {
            var slot = inventory.slots[i];

            PlayerPrefs.SetString($"{inventoryName}_Slot_{i}_ItemName", slot.itemName ?? "");
            PlayerPrefs.SetInt($"{inventoryName}_Slot_{i}_Count", slot.count);
        }

        PlayerPrefs.Save();
    }


    public void LoadInventoryData()
    {
        if (inventory == null) return;

        // 检查是否有任何存档数据
        bool hasSavedData = false;
        for (int i = 0; i < inventory.slots.Count; i++)
        {
            string savedItemName = PlayerPrefs.GetString($"{inventoryName}_Slot_{i}_ItemName", null);
            if (!string.IsNullOrEmpty(savedItemName))
            {
                hasSavedData = true;
                break;
            }
        }

        if (!hasSavedData)
        {
            Debug.Log("No saved inventory data found. Using initialized inventory.");
            return;
        }

        for (int i = 0; i < inventory.slots.Count; i++)
        {
            string itemName = PlayerPrefs.GetString($"{inventoryName}_Slot_{i}_ItemName", "");
            int count = PlayerPrefs.GetInt($"{inventoryName}_Slot_{i}_Count", 0);

            // 只有当存档数据有效时才覆盖槽位
            if (!string.IsNullOrEmpty(itemName) && count > 0)
            {
                var slot = new Inventory.Slot();
                slot.itemName = itemName;
                slot.count = count;

                var itemPrefab = GameManager.instance.itemManager.GetItemByName(itemName);
                if (itemPrefab != null)
                {
                    slot.icon = itemPrefab.GetComponent<Item>().data.icon;
                }

                inventory.slots[i] = slot; // 覆盖
            }
            // 否则保持槽位原数据不变
        }

        Refresh();
    }
    private void OnApplicationQuit()
    {
        SaveInventoryData();
    }

    private void OnDisable()
    {
        SaveInventoryData();
    }



}
