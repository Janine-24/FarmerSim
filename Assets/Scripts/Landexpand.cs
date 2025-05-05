using UnityEngine;

public class TileController : MonoBehaviour
{
    public int requiredLevel = 5;
    public bool isUnlocked = false;
    public CloudController cloudController;

    public void TryUnlockTile(int playerLevel)
    {
        if (playerLevel >= requiredLevel && !isUnlocked)
        {
            isUnlocked = true;
            cloudController.UnlockCloud();
        }
        else
        {
            Debug.Log("Haven't reach the conditions！");
        }
    }
    private void OnMouseDown()
    {
        // Here you can replace with your actual player level and coins
        int playerLevel = 1;

        if (!isUnlocked)
        {
            if (playerLevel >= requiredLevel)
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
            else
            {
                Debug.Log("Level too low to unlock.");
            }
        }
}
