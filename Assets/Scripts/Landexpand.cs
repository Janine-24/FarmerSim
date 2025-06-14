using TMPro;
using UnityEngine;

public class TileController : MonoBehaviour
{
    public int requiredLevel = 5; //can adjust in unity
    public bool isUnlocked = false;
    public CloudController cloudController;
    public TextMeshProUGUI hintText;
    public void Start()
    {
        TryUnlockTile(LevelSystem.Instance.level); //try access in level.cs
        hintText.gameObject.SetActive(false);
    }
    public void TryUnlockTile(int level)
    {
        if (isUnlocked) return;

        if (level >= requiredLevel)
        {
            isUnlocked = true;
            cloudController.UnlockCloud();
            Debug.Log("Land unlocked!");
        }
        else
        {
            //nothing
        }
    }
    private void OnMouseDown()
        {
            TryUnlockTile(LevelSystem.Instance.level);
            Debug.Log($"Land will be unlocked at Level {requiredLevel}. (Player current level: {LevelSystem.Instance.level})");
            ShowHint($"Land will be unlocked at Level {requiredLevel}. (Player current level: {LevelSystem.Instance.level})");
        }
    private void ShowHint(string message)
        {
            if (hintText == null) return;
            hintText.text = message;
            hintText.gameObject.SetActive(true);
            CancelInvoke(nameof(HideHint));// Cancel any ongoing invocation of HideHint
        Invoke(nameof(HideHint), 2f); // occur in 2 second
        }
    private void HideHint()
        {
            if (hintText != null)
            hintText.gameObject.SetActive(false);
        }

}
