using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Automatically links machine buttons to their products and sets up UI references
public class ButtonAutoLinker : MonoBehaviour
{
    public List<Product> products;                    // List of available products
    public GameObject buyingCanvas;                   // Reference to the buying machine UI
    public GameObject sellingCanvas;                  // Reference to the selling machine UI
    public BuyingMachineManager buyingMachine;        // Reference to the buying manager
    public SellingMachineManager sellingMachine;      // Reference to the selling manager

    private void Start()
    {
        // Find all buttons in the scene with the MachineButtonUI script
        MachineButtonUI[] allButtons = FindObjectsByType<MachineButtonUI>(FindObjectsSortMode.None);

        // Loop through each button and try to assign a product
        foreach (var button in allButtons)
        {
            string originalButtonName = button.name;
            string buttonName = originalButtonName.Trim();  // Remove extra spaces

            Debug.Log($"Try Matching button name: {originalButtonName} -> Button Name After Processing: {buttonName}");

            if (string.IsNullOrEmpty(buttonName))
            {
                Debug.LogWarning($"Button Name is Empty or Invalid, the Original Button Name: {originalButtonName}");
                continue;
            }

            bool foundMatch = false;

            // Try matching each product to the button by name
            foreach (var product in products)
            {
                Debug.Log($"Product Name: '{product.productName}', Compare Button Name: '{buttonName}'");

                if (product.productName.ToLower() == buttonName.ToLower())
                {
                    foundMatch = true;
                    Debug.Log($"Matching Success: {product.productName} With {buttonName}");

                    button.product = product;
                    button.isBuyingMachine = button.transform.IsChildOf(buyingCanvas.transform);  // Determine machine type
                    button.buyingMachine = buyingMachine;
                    button.sellingMachine = sellingMachine;

                    // Assign UI text references
                    TextMeshProUGUI[] texts = button.GetComponentsInChildren<TextMeshProUGUI>();
                    foreach (var txt in texts)
                    {
                        if (txt.name.ToLower().Contains("player"))
                            button.playerStockText = txt;
                        else if (txt.name.ToLower().Contains("machine"))
                            button.machineStockText = txt;
                    }

                    button.UpdateUI();  // Refresh the UI with stock data

                    // Automatically add the click handler script
                    var clickHandler = button.gameObject.AddComponent<ProductClickHandler>();
                    clickHandler.product = product;
                    clickHandler.isBuyingMachine = button.isBuyingMachine;

                    break;
                }
            }

            if (!foundMatch)
            {
                Debug.LogWarning($"No Matching Products Found: {buttonName}");
            }
        }

        // Print out all available products for debugging
        Debug.Log("Product List:");
        foreach (var p in products)
        {
            Debug.Log($"Product Name: {p.productName}");
        }
    }
}
