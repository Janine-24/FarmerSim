using UnityEngine;

// This class is for selecting a skin and updating the selected AnimatorOverrideController.
public class SkinSelector : MonoBehaviour
{
    // Store current selected skin's AnimatorOverrideController
    public AnimatorOverrideController overrideController;

    // This method sets the selected skin by assigning the overrideController to the SkinManager.
    public void UseSkin()
    {
        // Store the selected overrideController(skin) to the SkinManager's selectedOverrideController.
        SkinManager.selectedOverrideController = overrideController;

        // Log a message in the console to confirm the skin selection.
        Debug.Log("Selected override set!" + overrideController.name);
    }
}
