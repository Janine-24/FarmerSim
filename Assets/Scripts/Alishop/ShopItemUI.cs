using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    public Image icon;
    public TMP_Text nameText, priceText;
    public Button buyButton;

    ShopItem currentItem;
    ShopManager shopManager;

    public void Setup(ShopItem item, ShopManager manager)
    {
        currentItem = item;
        shopManager = manager;
        icon.sprite = item.icon;
        nameText.text = item.itemName;
        priceText.text = "$" + item.price;
        int playerLevel = LevelSystem.Instance.level;  //CHECK THE LEVEL SYSTEM
        buyButton.interactable = playerLevel >= item.requiredLevel;
        buyButton.onClick.AddListener(() => shopManager.TryPurchase(item));
    }
}