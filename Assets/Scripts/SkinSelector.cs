using UnityEngine;

public class SkinSelector : MonoBehaviour
{
    public AnimatorOverrideController overrideController;
    public string skinKeyName; // ✅ 加入这个字段

    public void UseSkin()
    {
        if (!IsSkinBought())
        {
            Debug.Log("❌ Skin not purchased. Cannot use.");
            return;
        }

        SkinManager.selectedOverrideController = overrideController;
        PlayerPrefs.SetString("SelectedSkinName", overrideController.name);
        PlayerPrefs.Save();

        Debug.Log("🎨 Selected override set: " + overrideController.name);
    }

    bool IsSkinBought()
    {
        return PlayerPrefs.GetInt(GetSkinKey(), 0) == 1;
    }

    string GetSkinKey()
    {
        return "SkinBought_" + skinKeyName;
    }
}
