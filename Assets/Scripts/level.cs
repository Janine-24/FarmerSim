using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;

public class LevelSystem : MonoBehaviour

{
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
        UpdateUI();
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

    void LevelUp()
    {
        level++;
        xpToNextLevel += 100; //  can increase XP requirement per level
        if (audioSource != null && levelUpSound != null)
        {
            audioSource.PlayOneShot(levelUpSound);
        }
        Debug.Log("Level Up! Now Level: " + level);
        if (levelUpPopup != null)
        {
            levelUpPopup.SetActive(true);
            Invoke("HidePopup", 2f);
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        levelText.text = "Level " + level;
        xpBar.maxValue = xpToNextLevel;
        xpBar.value = currentXP;
    }
}
