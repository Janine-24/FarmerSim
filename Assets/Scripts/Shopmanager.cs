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
    private GameObject currentDraggedObject;

    private void Start()
    {
        shopPanel.SetActive(false); 
    }
    public void OpenShop()
    {
        shopPanel.SetActive(true);
        PopulateShop();
        Debug.Log("Shop Opened");
    }
    public void CloseShop()
    {
        shopPanel.SetActive(false);
        Debug.Log("Shop Closed");

        if (currentDraggedObject != null) //prevent conflict when have next time purchase as they will check have null or not
        {
            Destroy(currentDraggedObject); //destroy the drag object when close shop
            currentDraggedObject = null; //reset reference to drag object
        }
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
        int currentLevel = LevelSystem.Instance.level;
        bool hasCoins = PlayerCoinManager.Instance.HasEnoughCoins(item.price);
        if (currentLevel >= item.requiredLevel && hasCoins)
        {
            PlayerCoinManager.Instance.SpendCoins(item.price);
            StartDragging(item);
            Debug.Log("Purchase:" + item.itemName);
        }
        else
        {
            Debug.Log("Cannot purchase. Need level " + item.requiredLevel + ", coins: " + item.price);
        }
    }

    void StartDragging(ShopItem item)
    {
        DragManager.Instance.StartDragging(item);
    }
}
