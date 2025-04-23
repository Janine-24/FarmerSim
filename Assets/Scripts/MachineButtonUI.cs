using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MachineButtonUI : MonoBehaviour
{
    public Product product;
    public TextMeshProUGUI playerStockText;
    public TextMeshProUGUI machineStockText;

    public bool isBuyingMachine = true;
    public BuyingMachineManager buyingMachine;
    public SellingMachineManager sellingMachine;

    public int amount = 1;

    private Button button;

    // ✅ 用于识别“上一次点击的按钮”
    private static MachineButtonUI lastClickedButton = null;

    private void Start()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnButtonClick);
        }
        UpdateUI();
    }

    public void OnButtonClick()
    {
        // ✅ 检查是否两次点击的是同一个按钮
        if (lastClickedButton == this)
        {
            Debug.Log($"🌀 第二次点击相同按钮，重置库存：{product.productName}");
            ResetStockToInitial();
            lastClickedButton = null; // 重置
            return;
        }

        lastClickedButton = this; // 记录当前点击按钮

        Debug.Log($"🛒 第一次点击：{product.productName} | {(isBuyingMachine ? "买" : "卖")}");

        bool success = false;

        if (isBuyingMachine && buyingMachine != null)
        {
            success = buyingMachine.BuyProduct(product, amount);
        }
        else if (!isBuyingMachine && sellingMachine != null)
        {
            success = sellingMachine.SellProduct(product, amount);
        }

        if (success)
        {
            UpdateUI();
        }
    }

    private void ResetStockToInitial()
    {
        if (isBuyingMachine)
        {
            var entry = buyingMachine.machineStock.Find(e => e.product == product);
            if (entry != null)
            {
                entry.stock = product.initialStock;
            }
        }
        else
        {
            var entry = sellingMachine.machineStock.Find(e => e.product == product);
            if (entry != null)
            {
                entry.stock = product.initialStock;
            }
        }

        UpdateUI();
    }

    public void UpdateUI()
    {
        if (playerStockText != null)
            playerStockText.text = $"{InventoryManager.Instance.GetProductQuantity(product)}"; // 只显示玩家库存数字

        if (isBuyingMachine && machineStockText != null)
        {
            var entry = buyingMachine.machineStock.Find(e => e.product == product);
            machineStockText.text = $"{entry?.stock ?? 0}"; // 只显示售卖机库存数字
        }
        else if (!isBuyingMachine && machineStockText != null)
        {
            var entry = sellingMachine.machineStock.Find(e => e.product == product);
            machineStockText.text = $"{entry?.stock ?? 0}"; // 只显示售卖机库存数字
        }
    }

}

