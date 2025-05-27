using UnityEngine;
using UnityEngine.UI;

public class SkinBuyButton : MonoBehaviour
{
    public int skinPrice = 1000; // 在 Inspector 设置价格
    public Button buyButton;

    private void Start()
    {
        buyButton.onClick.AddListener(BuySkin);
    }

    void BuySkin()
    {
        if (PlayerCoinManager.Instance.HasEnoughCoins(skinPrice))
        {
            PlayerCoinManager.Instance.SpendCoins(skinPrice);
            Debug.Log("Skin bought successfully！");
            buyButton.interactable = false; // 禁用按钮，表示已购买
            // TODO: 在这里添加更换皮肤的逻辑或保存购买状态
        }
        else
        {
            Debug.Log("Not enough coin！");
            // TODO: 可添加UI提示玩家金币不足
        }
    }
}
