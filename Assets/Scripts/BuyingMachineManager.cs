using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class BuyingMachineManager : MonoBehaviour
{
    [Header("Product Setup")]
    public List<Product> buyingProducts;
    public List<Button> buyingButtons;

    [Header("Selling Machine Reference")]
    public SellingMachineManager sellingMachineManager;

    [Header("Total Price Display")]
    public TextMeshProUGUI totalPriceText;
    private int totalPrice = 0;

    [Header("Player Coin Manager")]
    public PlayerCoinManager playerCoinManager;

    [Header("Feedback Text")]
    public TextMeshProUGUI spendFeedbackText;

    private void Start()
    {
        SetupButtons();
    }

    private void SetupButtons()
    {
        for (int i = 0; i < buyingButtons.Count; i++)
        {
            int index = i;
            UpdateButtonDisplay(index);
            buyingButtons[i].onClick.AddListener(() => OnProductClicked(index));
        }
    }

    private void OnProductClicked(int index)
    {
        Product product = buyingProducts[index];
        if (product.currentQuantity > 0)
        {
            product.currentQuantity--;
            totalPrice += product.price;
            UpdateButtonDisplay(index);
            UpdateTotalPriceDisplay();
        }
    }

    private void UpdateButtonDisplay(int index)
    {
        Product product = buyingProducts[index];
        Button button = buyingButtons[index];
        button.GetComponentInChildren<TextMeshProUGUI>().text = product.currentQuantity.ToString();
        button.image.sprite = product.productImage;
    }

    private void UpdateTotalPriceDisplay()
    {
        totalPriceText.text = "$" + totalPrice;
    }

    public void ConfirmBuy()
    {
        if (!playerCoinManager.HasEnoughCoins(totalPrice)) return;

        playerCoinManager.SpendCoins(totalPrice);

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
}
