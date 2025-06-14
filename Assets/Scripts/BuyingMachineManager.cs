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
        
        for (int i = 0; i < buyingProducts.Count; i++) //Record the number of items purchased
        {
            int boughtAmount = buyingProducts[i].originalQuantity - buyingProducts[i].currentQuantity;
            Debug.Log($"Bought {boughtAmount} of {buyingProducts[i].productName}");
            
            buyingProducts[i].originalQuantity = buyingProducts[i].currentQuantity; // Update originalQuantity in buying machine (set to current quantity)
            sellingMachineManager.sellingProducts[i].currentQuantity += boughtAmount;// Sync the quantity of product quantity purchased to the selling machine
            //sellingMachineManager.sellingProducts[i].originalQuantity = sellingMachineManager.sellingProducts[i].currentQuantity;// Update n sync originalQuantity in selling machine 
            Debug.Log($"Selling Machine Updated {sellingMachineManager.sellingProducts[i].productName}: " +
                $"OriginalQuantity: {sellingMachineManager.sellingProducts[i].originalQuantity}, " +
                $"CurrentQuantity: {sellingMachineManager.sellingProducts[i].currentQuantity}");

            sellingMachineManager.UpdateButtonDisplay(i); // Update selling machine UI
        }
        
        spendFeedbackText.text = $"You have spent: ${totalPrice}"; // Display the amount spent
        StartCoroutine(ClearSpendText());

        totalPrice = 0; // Reset the total price and update the display
        UpdateTotalPriceDisplay();
        sellingMachineManager.SyncToInventory();
        sellingMachineManager.SaveInventory(); //Sync n save the selling machine status

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

        Debug.Log("Refreshed all buying products to daily stock quantity.");

        // update UI
        for (int i = 0; i < buyingProducts.Count; i++)
        {
            UpdateButtonDisplay(i);
        }

        SaveInventory(); // Save new daily stock
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
                Debug.Log($"Loaded: {buyingProducts[i].productName}, current: {buyingProducts[i].currentQuantity}, original: {buyingProducts[i].originalQuantity}");
            }
        }

        //  update ui product quantity 
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