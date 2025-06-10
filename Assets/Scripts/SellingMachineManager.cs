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
        PlayerCoinManager.Instance.AddCoins(totalPrice);


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
            Debug.Log($"[SellingMachine] Saved: {sellingProducts[i].productName}, current: {sellingProducts[i].currentQuantity}, original: {sellingProducts[i].originalQuantity}");
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
        Inventory toolbar = GameManager.instance.player.inventoryManager.toolbar;

        foreach (Product product in sellingProducts)
        {
            int totalCount = 0;

            // 遍历背包
            foreach (Inventory.Slot slot in backpack.slots)
            {
                if (slot != null && slot.itemName == product.productName)
                {
                    totalCount += slot.count;
                }
            }

            // 遍历 Toolbar
            foreach (Inventory.Slot slot in toolbar.slots)
            {
                if (slot != null && slot.itemName == product.productName)
                {
                    totalCount += slot.count;
                }
            }

            product.currentQuantity = totalCount;
            product.originalQuantity = totalCount; // 可选：同时设置原始数量
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

        Inventory backpack = GameManager.instance.player.inventoryManager.backpack;
        Inventory toolbar = GameManager.instance.player.inventoryManager.toolbar;

        foreach (Product product in sellingProducts)
        {
            int remainingToAssign = product.currentQuantity;
            bool foundInBackpack = false;
            bool foundInToolbar = false;

            // 👜 尝试更新背包中的对应物品数量
            foreach (Inventory.Slot slot in backpack.slots)
            {
                if (slot != null && slot.itemName == product.productName)
                {
                    slot.count = remainingToAssign;
                    foundInBackpack = true;
                    remainingToAssign = 0;
                    break;
                }
            }

            // 🧰 如果背包没找到，尝试更新 Toolbar
            if (!foundInBackpack)
            {
                foreach (Inventory.Slot slot in toolbar.slots)
                {
                    if (slot != null && slot.itemName == product.productName)
                    {
                        slot.count = remainingToAssign;
                        foundInToolbar = true;
                        remainingToAssign = 0;
                        break;
                    }
                }
            }

            // 🆕 如果两个地方都没找到，插入到背包或 Toolbar 的空位
            if (!foundInBackpack && !foundInToolbar && remainingToAssign > 0)
            {
                var itemPrefab = GameManager.instance.itemManager.GetItemByName(product.productName);
                if (itemPrefab == null) continue;

                var itemComponent = itemPrefab.GetComponent<Item>();
                var newSlot = new Inventory.Slot
                {
                    itemName = product.productName,
                    count = remainingToAssign,
                    icon = product.productImage,
                    itemData = itemComponent.data
                };

                bool inserted = false;

                // 插入到背包空位
                for (int i = 0; i < backpack.slots.Count; i++)
                {
                    if (string.IsNullOrEmpty(backpack.slots[i].itemName))
                    {
                        backpack.slots[i] = newSlot;
                        inserted = true;
                        Debug.Log($"✅ 新增 {product.productName} 到背包 slot {i}");
                        break;
                    }
                }

                // 插入到 Toolbar 空位（如果背包没插入成功）
                if (!inserted)
                {
                    for (int i = 0; i < toolbar.slots.Count; i++)
                    {
                        if (string.IsNullOrEmpty(toolbar.slots[i].itemName))
                        {
                            toolbar.slots[i] = newSlot;
                            Debug.Log($"✅ 新增 {product.productName} 到 Toolbar slot {i}");
                            break;
                        }
                    }
                }
            }
        }

        GameManager.instance.uiManager.RefreshInventoryUI("backpack");
        GameManager.instance.uiManager.RefreshInventoryUI("toolbar");
    }



}
