using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Inventory;
using UnityEngine.UI;
using TMPro;

public class Toolbar_UI : MonoBehaviour
{
    public List<Slot_UI> toolbarSlots = new List<Slot_UI>();
    public TextMeshProUGUI selectedItemNameText;
    private Slot_UI selectedSlot;
    private int selectedSlotIndex = -1;

    private void Start()
    {
        if (GameManager.instance == null || GameManager.instance.player == null)
        {
            Debug.LogError("GameManager or Player is not ready. Cannot initialize inventory.");
            return;
        }

        SelectSlot(0); // choose fist coloum default

    }
    public Slot GetSelectedSlot()
    {
        if (selectedSlotIndex >= 0 && selectedSlotIndex < toolbarSlots.Count)
        {
            return GameManager.instance.player.inventoryManager.toolbar.slots[selectedSlotIndex];
        }
        return null;
    }

    public void ClearSelectedSlot()
    {
        if (selectedSlotIndex >= 0 && selectedSlotIndex < toolbarSlots.Count)
        {
            GameManager.instance.player.inventoryManager.toolbar.slots[selectedSlotIndex] = new Slot();
        }
    }


    private void Update()
    {
        CheckAlphaNumericKeys();
    }

    public void SelectSlot(Slot_UI slot)
    {
        SelectSlot(slot.slotID);
    }

    public void SelectSlot(int index)
    {
        if (index >= 0 && index < toolbarSlots.Count)
        {
            if (selectedSlot != null)
            {
                selectedSlot.SetHighlight(false); // Unhighlight the previous selection
            }

            selectedSlot = toolbarSlots[index]; // Set the new selected slot
            selectedSlotIndex = index; // Update the selected index
            selectedSlot.SetHighlight(true); // Highlight the new selection

            var slotData = GameManager.instance.player.inventoryManager.toolbar.slots[index];

            // 🧠 Show item name + durability (if tool)
            if (!string.IsNullOrEmpty(slotData.itemName))
            {
                if (slotData.itemData != null && slotData.itemData.itemType == ItemType.Tool && slotData.individualDurability.Count > 0)
                {
                    selectedItemNameText.text = $"{slotData.itemName} (Durability: {slotData.individualDurability[0]})";
                }
                else
                {
                    selectedItemNameText.text = slotData.itemName;
                }
            }
            else
            {
                selectedItemNameText.text = "Empty";
            }

            GameManager.instance.player.inventoryManager.toolbar.SelectSlot(index);
        }
        else
        {
            Debug.LogError("Selected index is out of bounds.");
        }
    }
    private void CheckAlphaNumericKeys()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectSlot(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectSlot(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectSlot(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectSlot(3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SelectSlot(4);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SelectSlot(5);
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SelectSlot(6);
        }
    }

    public int GetSelectedSlotIndex()
    {
        return selectedSlotIndex;
    }

    public void UseSelectedItem()
    {
        var slot = GetSelectedSlot(); // get toolbar selected item

        if (slot != null && slot.count > 0)
        {
            slot.count--;

            var sellingMachine = GameManager.instance.uiManager.GetComponent<SellingMachineManager>();
            foreach (var product in sellingMachine.sellingProducts)
            {
                if (product.productName == slot.itemName && product.currentQuantity > 0)
                {
                    product.currentQuantity--;
                    break;
                }
            }

            // Update the corresponding product quantity in Selling Machine
            var backpack = GameManager.instance.player.inventoryManager.backpack;
            foreach (var product in GameManager.instance.uiManager.GetComponent<SellingMachineManager>().sellingProducts)
            {
                if (product.productName == slot.itemName)
                {
                    product.currentQuantity = slot.count;
                    break;
                }
            }
            GameManager.instance.uiManager.RefreshAll();
        }
    }



}