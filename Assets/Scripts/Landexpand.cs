using UnityEngine;

public class TileController : MonoBehaviour
{
    public int requiredLevel = 5; //can adjust in unity
    public bool isUnlocked = false;
    public CloudController cloudController;
    public void Start()
    {
        TryUnlockTile(LevelSystem.Instance.level); //try access in level.cs
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
            Debug.Log($"Land will be unlocked at Level {requiredLevel}.");

        }
    }
    private void OnMouseDown()
        {
            TryUnlockTile(LevelSystem.Instance.level);
            Debug.Log($"Land will be unlocked at Level {requiredLevel}. (Player current level: {LevelSystem.Instance.level})");

    }
}
