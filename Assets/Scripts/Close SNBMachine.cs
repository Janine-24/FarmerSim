using UnityEngine;
using UnityEngine.SceneManagement;

public class LateSNB : MonoBehaviour
{
    // 引用 Buying 和 Selling Machine Manager
    public BuyingMachineManager buyingMachineManager;
    public SellingMachineManager sellingMachineManager;

    public void Loadmainpage()
    {
        Debug.Log("Go to main page");
        SceneManager.LoadScene("main page");
    }

    public void OnCloseButtonClicked()
    {
        // 保存购买数据
        if (buyingMachineManager != null)
        {
            buyingMachineManager.SaveInventory();
        }
        else
        {
            Debug.LogError("BuyingMachineManager 引用未设置！");
        }

        // 保存售出数据
        if (sellingMachineManager != null)
        {
            sellingMachineManager.SaveInventory();
        }
        else
        {
            Debug.LogError("SellingMachineManager 引用未设置！");
        }

        // 切换到主页面
        SceneManager.LoadScene("main page");
    }
}
