using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
<<<<<<< HEAD

}

=======
}
>>>>>>> f598a507318380309e9bf7a9f70fd9940e01adf6
