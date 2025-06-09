using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class BuyingMachineManager : MonoBehaviour
{
    [Header("All Products")]
    public List<Product> buyingProducts;
    public List<Button> buyingButtons;

    [Header("Selling Machine Reference")]
    public SellingMachineManager sellingMachineManager;

    [Header("Total Price Display")]
    public TextMeshProUGUI totalPriceText;
    private int totalPrice = 0;

    [Header("Feedback Text")]
    public TextMeshProUGUI spendFeedbackText;

    private void Start()
    {

        LoadInventory(); // load data first
        CheckAndRefreshDailyStock(); // check need to refresh or not
        MakeButtonsFunction();
    }
    private void MakeButtonsFunction()//connect ui button in buying machine (change quantity ,click on it able to function)
    {
        for (int i = 0; i < buyingButtons.Count; i+=1)//run{}theni+=1
        {
            int index = i;//save i into index avoid connect button wrongly(button save self index)
            UpdateButtonDisplay(index);
            buyingButtons[i].onClick.AddListener(() => OnProductClicked(index));//addlis-after click run onproductclicked
        }
    }

    private void OnProductClicked(int index)//click on button quantity decrease, add to total price
    {
        Product product = buyingProducts[index];//link to product
        if (product.currentQuantity > 0)
        {
            product.currentQuantity--;//product quantity minus 1
            totalPrice += product.price;//add product price to totalprice text
            UpdateButtonDisplay(index);//update word and image
            UpdateTotalPriceDisplay();//show in totalpricetext
        }
    }
    private void UpdateButtonDisplay(int index)//update button's image and word
    {
        Product product = buyingProducts[index];
        Button button = buyingButtons[index];
        button.GetComponentInChildren<TextMeshProUGUI>().text = product.currentQuantity.ToString();//find button text(ui is string nit number)
        button.image.sprite = product.productImage;//Replace the image on the button with the image product i put.
    }

    private void UpdateTotalPriceDisplay()
    {
        totalPriceText.text = "$" + totalPrice;
    }
    public void ConfirmBuy()
    {
        if (!PlayerCoinManager.Instance.HasEnoughCoins(totalPrice))
        {
            spendFeedbackText.text = "Not enough coins!";
            StartCoroutine(ClearSpendText());
            return;
        }

        PlayerCoinManager.Instance.SpendCoins(totalPrice);
        // 记录购买的物品数量
        for (int i = 0; i < buyingProducts.Count; i++)
        {
            // 计算购买的数量（购买的是 originalQuantity 和 currentQuantity 之间的差值）
            int boughtAmount = buyingProducts[i].originalQuantity - buyingProducts[i].currentQuantity;

            // 调试输出，查看每个商品的变化
            Debug.Log($"Bought {boughtAmount} of {buyingProducts[i].productName}");

            // 更新 buying machine 中的 originalQuantity（设置为当前数量）
            buyingProducts[i].originalQuantity = buyingProducts[i].currentQuantity;

            // 将购买的商品数量同步到 selling machine
            sellingMachineManager.sellingProducts[i].currentQuantity += boughtAmount;

            // 同步更新 selling machine 中的 originalQuantity
            sellingMachineManager.sellingProducts[i].originalQuantity = sellingMachineManager.sellingProducts[i].currentQuantity;

            // 调试输出，确保 selling machine 的商品数量已经更新
            Debug.Log($"Selling Machine Updated {sellingMachineManager.sellingProducts[i].productName}: " +
                $"OriginalQuantity: {sellingMachineManager.sellingProducts[i].originalQuantity}, " +
                $"CurrentQuantity: {sellingMachineManager.sellingProducts[i].currentQuantity}");

            // 更新 selling machine UI
            sellingMachineManager.UpdateButtonDisplay(i);
        }

        // 显示花费的金额反馈
        spendFeedbackText.text = $"You have spent: ${totalPrice}";
        StartCoroutine(ClearSpendText());

        // 重置总价格并更新显示
        totalPrice = 0;
        UpdateTotalPriceDisplay();
        sellingMachineManager.SyncToInventory();
        sellingMachineManager.SaveInventory(); // ✅ 同步保存卖出机状态
        
    }
    private IEnumerator ClearSpendText()
    {
        yield return new WaitForSeconds(2f);
        spendFeedbackText.text = "";
    }

    public void ResetBuyingMachine()
    {
        totalPrice = 0;
        for (int i = 0; i < buyingProducts.Count; i++)
        {
            buyingProducts[i].currentQuantity = buyingProducts[i].originalQuantity;
            UpdateButtonDisplay(i);
        }
        UpdateTotalPriceDisplay();
    }

    private void RefreshToDailyStock()
    {
        for (int i = 0; i < buyingProducts.Count; i++)
        {
            buyingProducts[i].currentQuantity = buyingProducts[i].dailyStockQuantity;
            buyingProducts[i].originalQuantity = buyingProducts[i].dailyStockQuantity;
        }

        Debug.Log("✅ Refreshed all buying products to daily stock quantity.");

        // 更新 UI
        for (int i = 0; i < buyingProducts.Count; i++)
        {
            UpdateButtonDisplay(i);
        }

        SaveInventory(); // 保存新的 daily stock
    }

    private void CheckAndRefreshDailyStock()
    {
        string lastRefreshDate = PlayerPrefs.GetString("LastStockRefreshDate", "");
        string today = System.DateTime.Now.ToString("yyyy-MM-dd");

        if (lastRefreshDate != today)
        {
            RefreshToDailyStock(); 
            PlayerPrefs.SetString("LastStockRefreshDate", today);
            PlayerPrefs.Save();
            Debug.Log("Daily stock has been refreshed.");
        }
        else
        {
            Debug.Log("Stock already refreshed today.");
        }
    }

    public void SaveInventory()
    {
        for (int i = 0; i < buyingProducts.Count; i++)
        {
            PlayerPrefs.SetInt($"Buying_Product_{i}_CurrentQuantity", buyingProducts[i].currentQuantity);
            PlayerPrefs.SetInt($"Buying_Product_{i}_OriginalQuantity", buyingProducts[i].originalQuantity);
            Debug.Log($"Saved: {buyingProducts[i].productName}, current: {buyingProducts[i].currentQuantity}, original: {buyingProducts[i].originalQuantity}");
        }
        PlayerPrefs.Save();
    }

    public void LoadInventory()
    {
        for (int i = 0; i < buyingProducts.Count; i++)
        {
            string currentKey = $"Buying_Product_{i}_CurrentQuantity";
            string originalKey = $"Buying_Product_{i}_OriginalQuantity";

            if (PlayerPrefs.HasKey(currentKey) && PlayerPrefs.HasKey(originalKey))
            {
                buyingProducts[i].currentQuantity = PlayerPrefs.GetInt(currentKey);
                buyingProducts[i].originalQuantity = PlayerPrefs.GetInt(originalKey);
                Debug.Log($"✅ Loaded: {buyingProducts[i].productName}, current: {buyingProducts[i].currentQuantity}, original: {buyingProducts[i].originalQuantity}");
            }
        }

        // ✅ 加这个：确保 UI 数量被正确更新
        for (int i = 0; i < buyingProducts.Count; i++)
        {
            UpdateButtonDisplay(i);
        }
    }
    private void OnApplicationQuit()
    {
        SaveInventory();
    }
    private void OnDisable()
    {
        SaveInventory();
    }

}