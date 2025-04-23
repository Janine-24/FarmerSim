using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

// Handles single and double clicks on machine buttons
public class ProductClickHandler : MonoBehaviour, IPointerClickHandler
{
    public Product product;               // The product linked to this button
    public bool isBuyingMachine;          // True if it's on the buying machine

    private int originalStock;            // Original stock saved at start
    private int clickCount = 0;           // Track click count
    private float clickTimeThreshold = 0.3f;  // Max time between clicks for double click
    private Coroutine clickCoroutine;     // Used to delay single click execution

    private BuyingMachineManager buyingMachine;
    private SellingMachineManager sellingMachine;

    private void Start()
    {
        buyingMachine = FindFirstObjectByType<BuyingMachineManager>();
        sellingMachine = FindFirstObjectByType<SellingMachineManager>();
        SaveOriginalStock();  // Save initial stock at start
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        clickCount++;
        if (clickCoroutine != null)
            StopCoroutine(clickCoroutine);
        clickCoroutine = StartCoroutine(ClickRoutine());
    }

    IEnumerator ClickRoutine()
    {
        yield return new WaitForSeconds(clickTimeThreshold);

        if (clickCount == 1)
        {
            HandleSingleClick();  // Normal buy/sell
        }
        else if (clickCount >= 2)
        {
            HandleDoubleClick();  // Reset stock
        }

        clickCount = 0;
    }

    // Save current machine stock as the original value
    void SaveOriginalStock()
    {
        if (isBuyingMachine)
        {
            originalStock = buyingMachine.GetStock(product);
        }
        else
        {
            originalStock = sellingMachine.GetStock(product);
        }
    }

    // On single click: buy or sell 1 unit
    void HandleSingleClick()
    {
        if (isBuyingMachine)
        {
            if (buyingMachine.BuyProduct(product, 1))
            {
                // Add the sold item to the selling machine (so player can sell it later)
                sellingMachine.AddStock(product, 1);
            }
        }
        else
        {
            sellingMachine.SellProduct(product, 1);
        }
    }

    // On double click: reset machine stock to the original value
    void HandleDoubleClick()
    {
        if (isBuyingMachine)
        {
            buyingMachine.SetStock(product, originalStock);
        }
        else
        {
            sellingMachine.SetStock(product, originalStock);
        }
    }
}
