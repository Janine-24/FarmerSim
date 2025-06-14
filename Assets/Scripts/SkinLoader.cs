using UnityEngine;

public class SkinLoader : MonoBehaviour
{
    public Animator animator; // 要控制的角色 Animator
    public AnimatorOverrideController[] allSkins;
    public AnimatorOverrideController defaultSkin; // 默认皮肤

    void Start()
    {
        string savedSkinName = PlayerPrefs.GetString("SelectedSkinName", "");

        AnimatorOverrideController selectedSkin = null;

        if (!string.IsNullOrEmpty(savedSkinName))
        {
            foreach (var skin in allSkins)
            {
                if (skin.name == savedSkinName)
                {
                    selectedSkin = skin;
                    break;
                }
            }
        }

        // 没找到保存的皮肤就使用默认皮肤
        if (selectedSkin == null && defaultSkin != null)
        {
            selectedSkin = defaultSkin;
            Debug.Log("No saved skin found, using default skin.");
        }

        // 应用皮肤
        if (selectedSkin != null && animator != null)
        {
            animator.runtimeAnimatorController = selectedSkin;
            SkinManager.selectedOverrideController = selectedSkin;
            Debug.Log("Loaded skin: " + selectedSkin.name);
        }
    }
}
