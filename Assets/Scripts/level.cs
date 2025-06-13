using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelSystem : MonoBehaviour
{
    public static LevelSystem Instance; //easy to use other script to visit LevelSystem by using LevelSystem.Instance
    public int level = 1;
    public int currentXP = 0;
    public int xpToNextLevel = 100;

    public TextMeshProUGUI levelText;
    public Slider xpBar;
    public AudioSource audioSource;
    public AudioClip xpSound;
    public AudioClip levelUpSound;
    public GameObject levelUpPopup;

    void Start()
    {
        int Level = LevelSystem.Instance.level;
        LoadLevelData();
        UpdateUI();
    }
    public void SaveLevel()
    {
        PlayerPrefs.SetInt("PlayerLevel", level);
    }

    // load level
    public void LoadLevel()
    {
        level = PlayerPrefs.GetInt("PlayerLevel", 1); // if has no level, set to 1
    }
    public void ForceSetLevel(int newLevel)
    {
        level = newLevel;
        SaveLevel();
        UpdateUI();
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this; //levelsystem
        else
            Destroy(gameObject);// avoid multiple level system
    }
    public void AddXP(int amount)
    {
        currentXP += amount;

        if (xpSound != null && audioSource != null)
            audioSource.PlayOneShot(xpSound);

        if (currentXP >= xpToNextLevel)
        {
            currentXP -= xpToNextLevel;
            LevelUp();
        }

        UpdateUI();
    }

    private System.Collections.IEnumerator HidePopupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (levelUpPopup != null)
            levelUpPopup.SetActive(false);
    }

    void LevelUp()
    {
        level++;
        xpToNextLevel += 100; // Increase required XP for next level

        if (audioSource != null && levelUpSound != null)
            audioSource.PlayOneShot(levelUpSound);

        Debug.Log("Level Up! Now Level: " + level);

        if (levelUpPopup != null)
        {
            levelUpPopup.SetActive(true);
            StartCoroutine(HidePopupAfterDelay(2f));
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        if (levelText != null)
            levelText.text = "Level " + level;

        if (xpBar != null)
        {
            xpBar.maxValue = xpToNextLevel;
            xpBar.value = currentXP;
        }
    }

    void HidePopup()
    {
        if (levelUpPopup != null)
            levelUpPopup.SetActive(false);
    }

    public void SaveLevelData()
    {
        PlayerPrefs.SetInt("PlayerLevel", level);
        PlayerPrefs.SetInt("PlayerCurrentXP", currentXP);
        PlayerPrefs.SetInt("PlayerXPToNextLevel", xpToNextLevel);
        PlayerPrefs.Save();
        Debug.Log("Level data saved");
    }

    public void LoadLevelData()
    {
        level = PlayerPrefs.GetInt("PlayerLevel", 1);
        currentXP = PlayerPrefs.GetInt("PlayerCurrentXP", 0);
        xpToNextLevel = PlayerPrefs.GetInt("PlayerXPToNextLevel", 100);
        Debug.Log($"✅ Level data loaded: Level {level}, XP {currentXP}/{xpToNextLevel}");
    }

    private void OnApplicationQuit()
    {
        SaveLevelData();
    }

    private void OnDisable()
    {
        SaveLevelData();
    }



}