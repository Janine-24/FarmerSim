using UnityEngine;
public static class SkinManager// This static class is responsible for managing the selected AnimatorOverrideController for skins.
{
    // This is a static variable to store the currently selected AnimatorOverrideController.
    // This variable can be accessed and modified from other scripts to set or get the skin that has been selected.
    public static AnimatorOverrideController selectedOverrideController = null;
}
