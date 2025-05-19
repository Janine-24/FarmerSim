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
        UpdateCoinDisplay(); // 游戏开始时初始化金币 UI 显示
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

    /// <summary>
    /// 花费金币（如果有足够的金币）
    /// Deduct coins from player if enough coins are available
    /// </summary>
    public void SpendCoins(int amount)
    {
        if (currentCoinAmount >= amount)
        {
            currentCoinAmount -= amount;
            UpdateCoinDisplay(); // 更新金币显示
        }
        // 注意：金币不足时此处不处理 UI 提示，由调用者处理
    }

    /// <summary>
    /// 增加金币
    /// Adds coins to the player's total
    /// </summary>
    public void AddCoins(int amount)
    {
        currentCoinAmount += amount;
        UpdateCoinDisplay();
    }

    /// <summary>
    /// 检查是否有足够的金币
    /// Checks if the player has enough coins
    /// </summary>
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