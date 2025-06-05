using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class SellingMachineManager : MonoBehaviour
{
    [Header("Product Setup")]
    public List<Product> sellingProducts;
    public List<Button> sellingButtons;

    [Header("Total Price Display")]
    public TextMeshProUGUI totalPriceText;
    private int totalPrice = 0;

    [Header("Player Coin Manager")]
    public PlayerCoinManager playerCoinManager;

    [Header("Feedback Text")]
    public TextMeshProUGUI earnFeedbackText;

    private void Start()
    {
        LoadInventory();          // ✅ 加载本地保存的数据
        SetupButtons();           // ✅ 设置按钮
        StartCoroutine(DelayedSync());
    }

    private IEnumerator DelayedSync()
    {
        yield return null; // 延迟一帧，等 GameManager 和背包加载完成
        SyncFromInventory();
    }
    private void SetupButtons()
    {
        for (int i = 0; i < sellingButtons.Count; i++)
        {
            int index = i;
            UpdateButtonDisplay(index);
            sellingButtons[i].onClick.AddListener(() => OnProductClicked(index));
        }
    }

    private void OnProductClicked(int index)
    {
        Product product = sellingProducts[index];
        if (product.currentQuantity > 0)
        {
            product.currentQuantity--;
            totalPrice += product.price;
            UpdateButtonDisplay(index);
            UpdateTotalPriceDisplay();
        }
    }

    public void UpdateButtonDisplay(int index)
    {
        Product product = sellingProducts[index];
        Button button = sellingButtons[index];
        button.GetComponentInChildren<TextMeshProUGUI>().text = product.currentQuantity.ToString();
        button.image.sprite = product.productImage;
    }

    private void UpdateTotalPriceDisplay()
    {
        totalPriceText.text = "$" + totalPrice;
    }

    private IEnumerator ClearEarnText()
    {
        yield return new WaitForSeconds(2f);
        earnFeedbackText.text = "";
    }

    public void ConfirmSell()
    {
        playerCoinManager.AddCoins(totalPrice);

        for (int i = 0; i < sellingProducts.Count; i++)
        {
            sellingProducts[i].originalQuantity = sellingProducts[i].currentQuantity;
        }

        earnFeedbackText.text = $"You have earned: ${totalPrice}";
        StartCoroutine(ClearEarnText());

        totalPrice = 0;
        UpdateTotalPriceDisplay();

        SyncToInventory();   // 关键时刻同步
        SaveInventory();     // 并保存本地数据
    }

    public void ResetSellingMachine()
    {
        totalPrice = 0;

        for (int i = 0; i < sellingProducts.Count; i++)
        {
            sellingProducts[i].currentQuantity = sellingProducts[i].originalQuantity;
            UpdateButtonDisplay(i);
        }
        UpdateTotalPriceDisplay();

        SyncToInventory(); // ✅ 重置后同步回 Inventory
    }

    public void SaveInventory()
    {
        for (int i = 0; i < sellingProducts.Count; i++)
        {
            PlayerPrefs.SetInt($"Selling_Product_{i}_CurrentQuantity", sellingProducts[i].currentQuantity);
            PlayerPrefs.SetInt($"Selling_Product_{i}_OriginalQuantity", sellingProducts[i].originalQuantity);
            Debug.Log($"✅ [SellingMachine] Saved: {sellingProducts[i].productName}, current: {sellingProducts[i].currentQuantity}, original: {sellingProducts[i].originalQuantity}");
        }
        PlayerPrefs.Save();
    }

    public void LoadInventory()
    {
        for (int i = 0; i < sellingProducts.Count; i++)
        {
            string currentKey = $"Selling_Product_{i}_CurrentQuantity";
            string originalKey = $"Selling_Product_{i}_OriginalQuantity";

            if (PlayerPrefs.HasKey(currentKey) && PlayerPrefs.HasKey(originalKey))
            {
                sellingProducts[i].currentQuantity = PlayerPrefs.GetInt(currentKey);
                sellingProducts[i].originalQuantity = PlayerPrefs.GetInt(originalKey);
                Debug.Log($"✅ [SellingMachine] Loaded: {sellingProducts[i].productName}, current: {sellingProducts[i].currentQuantity}, original: {sellingProducts[i].originalQuantity}");
            }
        }

        for (int i = 0; i < sellingProducts.Count; i++)
        {
            UpdateButtonDisplay(i);
        }
    }

    private void OnApplicationQuit()
    {
        SaveInventory();
        SyncToInventory();
    }

    private void OnDisable()
    {
        SaveInventory();
    }


    // ✅ 新增：同步 Inventory → Selling Machine
    public void SyncFromInventory()
    {
        Debug.Log("✅ 正在执行 SyncFromInventory()");
        Inventory backpack = GameManager.instance.player.inventoryManager.backpack;
        
        foreach (Inventory.Slot slot in backpack.slots)
        {
            Debug.Log($"🧪 背包槽位: itemName = {slot?.itemName}, count = {slot?.count}");
        }

        

        foreach (Product product in sellingProducts)
        {
            foreach (Inventory.Slot slot in backpack.slots)
            {
                if (slot != null && slot.itemName == product.productName)
                {
                    product.currentQuantity = slot.count;
                    product.originalQuantity = slot.count; // 可选：也同步原始值
                    break;
                }
            }
        }

        for (int i = 0; i < sellingProducts.Count; i++)
        {
            UpdateButtonDisplay(i);
        }
    }

    // ✅ 新增：同步 Selling Machine → Inventory
   public void SyncToInventory()
{
        Debug.Log("✅ 正在执行 SyncToInventory()");
        if (GameManager.instance == null)
    {
        Debug.LogError("❌ GameManager.instance is null");
        return;
    }
    if (GameManager.instance.player == null)
    {
        Debug.LogError("❌ GameManager.player is null");
        return;
    }
    if (GameManager.instance.player.inventoryManager == null)
    {
        Debug.LogError("❌ InventoryManager is null");
        return;
    }
    if (GameManager.instance.player.inventoryManager.backpack == null)
    {
        Debug.LogError("❌ Backpack is null");
        return;
    }

    Inventory backpack = GameManager.instance.player.inventoryManager.backpack;

    foreach (Product product in sellingProducts)
    {
        foreach (Inventory.Slot slot in backpack.slots)
        {
            if (slot != null && slot.itemName == product.productName)
            {
                slot.count = product.currentQuantity;
                break;
            }
        }
    }

    GameManager.instance.uiManager.RefreshInventoryUI("backpack");
}

}
