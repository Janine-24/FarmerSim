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
        LoadInventory();          // Loading locally saved data
        MakeButtonsFunction();
        StartCoroutine(DelayedSync());
        
    }

    private IEnumerator DelayedSync()
    {
        yield return null; //Delay one frame until the GameManager and backpack are loaded
        SyncFromInventory();
    }
    private void MakeButtonsFunction()
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

        if (product.currentQuantity <= 0)
        {
            Debug.Log("❌ No more items to sell!");
            return;
        }

        product.currentQuantity -= 1; // 只是减少 UI 上的显示
        totalPrice += product.price;

        UpdateButtonDisplay(index);
        UpdateTotalPriceDisplay();
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

        SyncToInventory();
        SaveInventory();
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

    private bool hasInitializedOriginalQuantities = false;

    //sync Inventory → Selling Machine
    public void SyncFromInventory()
    {
        Debug.Log("Processing SyncFromInventory()");

        Inventory backpack = GameManager.instance.player.inventoryManager.backpack;
        Inventory toolbar = GameManager.instance.player.inventoryManager.toolbar;

        foreach (Product product in sellingProducts)
        {
            int totalCount = 0;

            foreach (Inventory.Slot slot in backpack.slots)
                if (slot != null && slot.itemName == product.productName)
                    totalCount += slot.count;

            foreach (Inventory.Slot slot in toolbar.slots)
                if (slot != null && slot.itemName == product.productName)
                    totalCount += slot.count;

            product.currentQuantity = totalCount;

            // ✅ 只在第一次时设定 originalQuantity
            if (!hasInitializedOriginalQuantities)
            {
                product.originalQuantity = totalCount;
            }
        }

        hasInitializedOriginalQuantities = true;

        for (int i = 0; i < sellingProducts.Count; i++)
        {
            UpdateButtonDisplay(i);
        }
    }



    // Sync Selling Machine → Inventory
    public void SyncToInventory()
    {
        Debug.Log("Processing SyncToInventory()");

        Inventory backpack = GameManager.instance.player.inventoryManager.backpack;
        Inventory toolbar = GameManager.instance.player.inventoryManager.toolbar;

        foreach (Product product in sellingProducts)
        {
            int remainingToAssign = product.currentQuantity;
            bool foundInBackpack = false;
            bool foundInToolbar = false;
            int existingInToolbar = 0;

            foreach (var slot in toolbar.slots)
            {
                if (slot != null && slot.itemName == product.productName)
                {
                    existingInToolbar += slot.count;
                    slot.count = 0;
                }
            }
            //remainingToAssign -= existingInToolbar; 不知道有没有用but不要动先
            if (remainingToAssign < 0) remainingToAssign = 0;

            // update that product quantity in inventory
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

            // if inventory can't find try update toolbar
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

            // input product into empty coloum of toolbar or inventory if can't find in both place
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

                // input into empty coloum of inventory
                for (int i = 0; i < backpack.slots.Count; i++)
                {
                    if (string.IsNullOrEmpty(backpack.slots[i].itemName))
                    {
                        backpack.slots[i] = newSlot;
                        inserted = true;
                        Debug.Log($"Add {product.productName} to inventory slot {i}");
                        break;
                    }
                }

                // if inventory input unsuccessfully,inout toolbar empty coloum
                if (!inserted)
                {
                    for (int i = 0; i < toolbar.slots.Count; i++)
                    {
                        if (string.IsNullOrEmpty(toolbar.slots[i].itemName))
                        {
                            toolbar.slots[i] = newSlot;
                            Debug.Log($"Add {product.productName} to Toolbar slot {i}");
                            break;
                        }
                    }
                }
            }
        }

        GameManager.instance.uiManager.RefreshInventoryUI("toolbar");
        GameManager.instance.uiManager.RefreshInventoryUI("backpack");

    }
}