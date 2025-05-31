using TMPro;
using UnityEngine;
using System.Collections;

public class CoinTextBinder : MonoBehaviour
{
    public TextMeshProUGUI coinTextUI;

    private void Start()
    {
        StartCoroutine(DelayedBind());
    }

    private IEnumerator DelayedBind()
    {
        int maxTries = 10;
        while ((PlayerCoinManager.Instance == null || coinTextUI == null) && maxTries-- > 0)
        {
            yield return null;
        }

        if (PlayerCoinManager.Instance != null && coinTextUI != null)
        {
            PlayerCoinManager.Instance.SetCoinText(coinTextUI);
            Debug.Log("Coin Text bound successfully.");
        }
        else
        {
            Debug.LogWarning("Coin Text binding failed: Instance or UI is still null after retries.");
        }
    }

}