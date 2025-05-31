using UnityEngine;

public class XPButton : MonoBehaviour
{
    public LevelSystem levelSystem;

    public void AddXPOnClick()
    {
        levelSystem.AddXP(10); // Adjust amount wanted
    }
}

