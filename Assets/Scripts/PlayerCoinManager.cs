using TMPro;
using UnityEngine;

/// <summary>
/// 管理玩家金币的脚本，全局单例，支持场景切换保持金币数据不丢失
/// Manages player's coins using a singleton that persists across scenes
/// </summary>
public class PlayerCoinManager : MonoBehaviour
{
    // 单例模式 Singleton instance
    public static PlayerCoinManager Instance { get; private set; }

    // 当前金币数量 Initial amount of coins the player starts with
    public int currentCoinAmount = 1000;

    // 用于在 UI 上显示金币数量的 Text 元素（TextMeshPro 组件）
    public TextMeshProUGUI coinText;

    private void Awake()
    {
        // 保证场景中只存在一个实例，并在切换场景时不被销毁
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 如果已有实例则销毁重复的
            return;
        }
        Debug.Log("PlayerCoinManager Awake called.");

        Instance = this; // 设置当前实例为单例
        DontDestroyOnLoad(gameObject); // 保持在所有场景中不被销毁
    }

    private void Start()
    {
        LoadCoins();            // 加载上次保存的金币
        UpdateCoinDisplay();    // 更新 UI
    }

    private void OnApplicationQuit()
    {
        SaveCoins();            // 应用退出时保存金币
    }

    private void OnDisable()
    {
        SaveCoins();            // 场景卸载或物体禁用时保存金币
    }

    public void SaveCoins()
    {
        PlayerPrefs.SetInt("PlayerCoins", currentCoinAmount);
        PlayerPrefs.Save();
        Debug.Log("✅ Coins saved: " + currentCoinAmount);
    }

    public void LoadCoins()
    {
        currentCoinAmount = PlayerPrefs.GetInt("PlayerCoins", currentCoinAmount); // 默认使用已有值
        Debug.Log("✅ Coins loaded: " + currentCoinAmount);
    }


    /// <summary>
    /// 更新 UI 显示当前金币数
    /// Updates the UI display with the current coin amount
    /// </summary>
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

    /// <summary>
    /// 设置 coinText 的引用（在新场景中绑定新的 UI）
    /// Sets the coin text UI reference, typically when a new scene is loaded
    /// </summary>
    public void SetCoinText(TextMeshProUGUI newText)
    {
        coinText = newText;
        UpdateCoinDisplay();
    }


}