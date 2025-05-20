using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;

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
        SetupButtons();
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
            // 卖出后将当前值设为新的 originalQuantity
            sellingProducts[i].originalQuantity = sellingProducts[i].currentQuantity;
            earnFeedbackText.text = $"You have earned: ${totalPrice}";
            StartCoroutine(ClearEarnText());
        }

        totalPrice = 0;
        UpdateTotalPriceDisplay();
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
            PlayerPrefs.SetInt($"{"Selling"}_Product_{i}_CurrentQuantity", sellingProducts[i].currentQuantity);
            PlayerPrefs.SetInt($"{"Selling"}_Product_{i}_OriginalQuantity", sellingProducts[i].originalQuantity);
        }
        PlayerPrefs.Save();
    }
    public void LoadInventory()
    {
        for (int i = 0; i < sellingProducts.Count; i++)
        {
            string currentKey = $"{"Selling"}_Product_{i}_CurrentQuantity";
            string originalKey = $"{"Selling"}_Product_{i}_OriginalQuantity";

            if (PlayerPrefs.HasKey(currentKey) && PlayerPrefs.HasKey(originalKey))
            {
                sellingProducts[i].currentQuantity = PlayerPrefs.GetInt(currentKey);
                sellingProducts[i].originalQuantity = PlayerPrefs.GetInt(originalKey);
            }
        }
    }
    private void OnApplicationQuit()
    {
        SaveInventory();
    }

}