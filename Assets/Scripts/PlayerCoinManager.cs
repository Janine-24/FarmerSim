using TMPro;
using UnityEngine;

public class PlayerCoinManager : MonoBehaviour
{
    public static PlayerCoinManager Instance { get; private set; }

    public int currentCoinAmount = 10000;
    public TextMeshProUGUI coinText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);  // 如果已有实例，销毁当前重复实例
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // 切换场景时不销毁
        LoadCoins();
    }

    private void Start()
    {
        UpdateCoinDisplay();
    }

    private void OnApplicationQuit()
    {
        SaveCoins();
    }

    private void OnDisable()
    {
        SaveCoins();
    }

    public void SaveCoins()
    {
        PlayerPrefs.SetInt("PlayerCoins", currentCoinAmount);
        PlayerPrefs.Save();
    }

    public void LoadCoins()
    {
        currentCoinAmount = PlayerPrefs.GetInt("PlayerCoins", currentCoinAmount);
    }

    public void UpdateCoinDisplay()
    {
        if (coinText != null)
            coinText.text = "$" + currentCoinAmount;
    }

    public void SpendCoins(int amount)
    {
        if (currentCoinAmount >= amount)
        {
            currentCoinAmount -= amount;
            SaveCoins();
            UpdateCoinDisplay();
        }
    }

    public void AddCoins(int amount)
    {
        currentCoinAmount += amount;
        SaveCoins();
        UpdateCoinDisplay();
    }

    public bool HasEnoughCoins(int amount)
    {
        return currentCoinAmount >= amount;
    }

    public void SetCoinText(TextMeshProUGUI newText)
    {
        coinText = newText;
        UpdateCoinDisplay();
    }
}
