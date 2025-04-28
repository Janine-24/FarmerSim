using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class BuyingMachineManager : MonoBehaviour
{
    [Header("Product Setup")]
    [SerializeField]
    public List<Product> buyingProducts;
    public List<Button> buyingButtons;

    [Header("Selling Machine Reference")]
    public SellingMachineManager sellingMachineManager; // 要拖拽 SellingMachineManager 进来

    [Header("Total Price Display")]
    public TextMeshProUGUI totalPriceText;
    private int totalPrice = 0;

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

            // 卖的机器数量 +1
            sellingMachineManager.sellingProducts[index].currentQuantity++;

            UpdateButtonDisplay(index);
            sellingMachineManager.UpdateButtonDisplay(index); // 更新selling那边
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

    public void ResetBuyingMachine()
    {
        totalPrice = 0;
        for (int i = 0; i < buyingProducts.Count; i++)
        {
            buyingProducts[i].currentQuantity = buyingProducts[i].originalQuantity;
            UpdateButtonDisplay(i);

            // Reset时要同步减回去在selling machine上加上去的数量
            sellingMachineManager.sellingProducts[i].currentQuantity = sellingMachineManager.sellingProducts[i].originalQuantity;
            sellingMachineManager.UpdateButtonDisplay(i);
        }
        UpdateTotalPriceDisplay();
    }
}
