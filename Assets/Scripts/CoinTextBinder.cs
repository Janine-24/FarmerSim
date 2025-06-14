using UnityEngine;
using System.Collections;
using TMPro;

public class CoinTextBinder : MonoBehaviour
{
    public TextMeshProUGUI coinTextUI;

    private IEnumerator Start()
    {
        if (coinTextUI == null)
            coinTextUI = GetComponentInChildren<TextMeshProUGUI>();

        float timeout = 2f;
        float elapsed = 0f;

        while ((PlayerCoinManager.Instance == null || coinTextUI == null) && elapsed < timeout)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (PlayerCoinManager.Instance != null && coinTextUI != null)
        {
            PlayerCoinManager.Instance.SetCoinText(coinTextUI);
            Debug.Log("CoinTextBinder: Coin text bound successfully.");
        }
        else
        {
            Debug.LogError("CoinTextBinder: 绑定失败，请确保PlayerCoinManager存在且coinTextUI设置正确。");
        }
    }
}