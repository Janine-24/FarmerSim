using UnityEngine;
using System.Collections.Generic;

public class WarehouseManager : MonoBehaviour
{
    public static WarehouseManager Instance;

    private Dictionary<string, int> storedItems = new ();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddItem(string itemName, int count)
    {
        if (!storedItems.ContainsKey(itemName))
            storedItems[itemName] = 0;

        storedItems[itemName] += count;
        Debug.Log($"Stored {itemName}: {storedItems[itemName]}");
    }
}
