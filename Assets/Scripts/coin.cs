using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public int coinCount = 0;
    public TextMeshProUGUI coinText;
    public AudioSource audioSource;
    public AudioClip coinSound;

    void Start()
    {
        UpdateCoinUI();
    }

    public void AddCoins(int amount)
    {
        coinCount += amount;
        UpdateCoinUI();

        if (audioSource != null && coinSound != null)
        {
            audioSource.PlayOneShot(coinSound);
        }
    }

    public void SpendCoins(int amount)
    {
        if (coinCount >= amount)
        {
            coinCount -= amount;
            UpdateCoinUI();
        }
        else
        {
            Debug.Log("Not enough coins!");
        }
    }

    void UpdateCoinUI()
    {
        coinText.text = coinCount.ToString();
    }
}
