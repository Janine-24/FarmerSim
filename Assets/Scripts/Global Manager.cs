using UnityEngine;
using System.Collections.Generic;

public class GlobalItemManager : MonoBehaviour
{
    public static GlobalItemManager instance;

    [System.Serializable]
    public class GlobalItem
    {
        public string itemName;
        public int quantity;
        public Sprite icon;
    }

    public List<GlobalItem> items = new List<GlobalItem>();

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    public void ModifyQuantity(string itemName, int amount)
    {
        var item = items.Find(i => i.itemName == itemName);
        if (item != null)
        {
            item.quantity += amount;
            if (item.quantity < 0) item.quantity = 0;
        }
    }

    public int GetQuantity(string itemName)
    {
        var item = items.Find(i => i.itemName == itemName);
        return item != null ? item.quantity : 0;
    }
}
