using UnityEngine;
using UnityEngine.UI;

public class SkinBuyButton : MonoBehaviour
{
    public int skinPrice = 1000;
    public Button buyButton;
    public string skinKeyName; // ✅ 加入这个字段

    private void Start()
    {
        if (IsSkinBought())
        {
            buyButton.interactable = false;
        }

        buyButton.onClick.AddListener(BuySkin);
    }

    void BuySkin()
    {
        if (PlayerCoinManager.Instance.HasEnoughCoins(skinPrice))
        {
            PlayerCoinManager.Instance.SpendCoins(skinPrice);
            Debug.Log("Skin bought successfully!");
            buyButton.interactable = false;

            PlayerPrefs.SetInt(GetSkinKey(), 1);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.Log("Not enough coins!");
        }
    }

    bool IsSkinBought()
    {
        return PlayerPrefs.GetInt(GetSkinKey(), 0) == 1;
    }

    string GetSkinKey()
    {
        return "SkinBought_" + skinKeyName;
    }
}
