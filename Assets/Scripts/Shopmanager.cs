using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    public GameObject shopItemPrefab;
    public GameObject shopPanel;
    public LevelSystem levelSystem;
    public Transform contentPanel;
    public List<ShopItem> allItems;
    public int playerLevel = 1;
    public int playerMoney = 1000;

    private void Start()
    {
        shopPanel.SetActive(false); 
    }
    public void OpenShop()
    {
        shopPanel.SetActive(true);
        PopulateShop();
    }

    public void CloseShop()
    {
            shopPanel.SetActive(false);
    }

    void PopulateShop()
    {
        foreach (var item in allItems)
        {
            GameObject go = Instantiate(shopItemPrefab, contentPanel);
            go.GetComponent<ShopItemUI>().Setup(item, this);

        }
    }

    public void TryPurchase(ShopItem item)
    {
        if (playerLevel >= item.requiredLevel && playerMoney >= item.price)
        {
            playerMoney -= item.price;
            StartDragging(item);
            Debug.Log("Purchase:" + item.itemName);
        }
        else
        {
            Debug.Log("Level or Money insufficient");
        }
    }

    void StartDragging(ShopItem item)
    {
        GameObject dragging = Instantiate(item.prefabToPlace);
        dragging.AddComponent<DragToPlace>().isAnimal = item.itemType == ShopItemType.Animal;
    }
}
