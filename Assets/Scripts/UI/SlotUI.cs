using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Slot_UI : MonoBehaviour
{
    public int slotID = -1;
    public string itemName;

    public Image itemIcon;
    public TextMeshProUGUI quantityText;
    public GameObject highlight;

    public Inventory inventory;

    public void SetItem(Inventory.Slot slot)
    {
        if (slot != null)
        {
            itemName = slot.itemName; // Check if slot is properly passed
            itemIcon.sprite = slot.icon;
            itemIcon.color = new Color(1, 1, 1, 1);
            quantityText.text = slot.count.ToString();
        }
        else
        {
            Debug.LogError("Slot is null in SetItem.");
        }
    }

    public void SetEmpty()
    {
        itemIcon.sprite = null;
        itemIcon.color = new Color(1, 1, 1, 0);
        quantityText.text = "";
    }

    public void SetHighlight(bool isOn)
    {
        if (highlight != null)
        {
            highlight.SetActive(isOn);
        }
    }
}

