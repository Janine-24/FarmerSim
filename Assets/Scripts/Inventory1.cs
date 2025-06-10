using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Inventory;

[System.Serializable]
public class Inventory
{
    [System.Serializable]
    public class Slot
    {
        public string itemName;
        public int count;
        public int maxAllowed;
        public ItemData itemData;
        public Sprite icon;
        public List<int> individualDurability = new List<int>();


        public Slot()
        {
            itemName = "";
            count = 0;
            maxAllowed = 99;
        }

        public bool IsEmpty
        {
            get
            {
                if (itemName == "" && count == 0)
                {
                    return true;
                }

                return false;
            }
        }

        public bool CanAddItem(string itemName)
        {
            if (this.itemName == itemName && count < maxAllowed)
            {
                return true;
            }

            return false;
        }

        public void AddItem(Item item)
        {
            if (count == 0)
            {
                this.itemName = item.data.itemName;
                this.icon = item.data.icon;
                this.itemData = item.data;
            }

            count++;

            // ✅ Always add durability if tool
            if (item.data.itemType == ItemType.Tool)
            {
                individualDurability.Add(item.data.durability);
            }
        }


        public void AddItem(string itemName, Sprite icon, int maxAllowed)
        {
            this.itemName = itemName;
            this.icon = icon;
            this.maxAllowed = maxAllowed;
            this.itemData = GameManager.instance.itemManager.GetItemByName(itemName).data; // <-- Add this
            count++;
        }

        public void RemoveItem()
        {
            if (count > 0)
            {
                count--;
                if (individualDurability.Count > 0)
                {
                    individualDurability.RemoveAt(0);
                }

                if (count == 0)
                {
                    icon = null;
                    itemName = "";
                    itemData = null;
                }
            }
        }
    }

    public List<Slot> slots = new List<Slot>();
    public Slot selectedSlot = null;

    public Inventory(int numSlots)
    {
        for (int i = 0; i < numSlots; i++)
        {
            slots.Add(new Slot());
        }
    }

    public void Add(Item item)
    {
        foreach (Slot slot in slots)
        {
            if (slot.itemName == item.data.itemName && slot.CanAddItem(item.data.itemName))
            {
                slot.AddItem(item);
                return;
            }
        }

        foreach (Slot slot in slots)
        {
            if (slot.itemName == "")
            {
                slot.AddItem(item);
                return;
            }
        }
    }

    public void Remove(int index)
    {
        slots[index].RemoveItem();
    }

    public void Remove(int index, int count)
    {
        if (slots[index].count >= count)
        {
            for (int i = 0; i < count; i++)
            {
                Remove(index);
            }
        }
    }

    public void MoveSlot(int fromIndex, int toIndex, Inventory toInventory, int numToMove = 1)
    {
        if (slots != null && slots.Count > 0)
        {
            Slot fromSlot = slots[fromIndex];
            Slot toSlot = toInventory.slots[toIndex];

            // Check if stacking is possible
            if (!toSlot.IsEmpty && toSlot.itemName == fromSlot.itemName && toSlot.count + numToMove <= toSlot.maxAllowed)
            {
                for (int i = 0; i < numToMove; i++)
                {
                    toSlot.count++;
                    fromSlot.RemoveItem();
                }
            }
            else if (toSlot.IsEmpty)
            {
                // Copy full data over
                toSlot.itemName = fromSlot.itemName;
                toSlot.icon = fromSlot.icon;
                toSlot.itemData = fromSlot.itemData; 
                toSlot.maxAllowed = fromSlot.maxAllowed;

                toSlot.count = 0; // start from 0, then add

                for (int i = 0; i < numToMove; i++)
                {
                    toSlot.count++;
                    fromSlot.RemoveItem();
                }
            }
            else
            {
                // Swap items
                string tempName = toSlot.itemName;
                Sprite tempIcon = toSlot.icon;
                int tempCount = toSlot.count;
                int tempMax = toSlot.maxAllowed;
                ItemData tempData = toSlot.itemData;

                toSlot.itemName = fromSlot.itemName;
                toSlot.icon = fromSlot.icon;
                toSlot.count = fromSlot.count;
                toSlot.maxAllowed = fromSlot.maxAllowed;
                toSlot.itemData = fromSlot.itemData;

                fromSlot.itemName = tempName;
                fromSlot.icon = tempIcon;
                fromSlot.count = tempCount;
                fromSlot.maxAllowed = tempMax;
                fromSlot.itemData = tempData;
            }
        }
    }
    public void SelectSlot(int index)
    {
        if (slots != null && slots.Count > 0)
        {
            selectedSlot = slots[index];
        }
    }
}


