using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class SellingMachineManager : MonoBehaviour
{
    [Header("Product Setup")]
    [SerializeField]
    public List<Product> sellingProducts;
    public List<Button> sellingButtons;

    [Header("Total Price Display")]
    public TextMeshProUGUI totalPriceText;
    private int totalPrice = 0;

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
        totalPriceText.text = "Total: $" + totalPrice;
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
}
