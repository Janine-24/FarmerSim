using UnityEngine;
using UnityEngine.UI;

public class ChangeOriSkin : MonoBehaviour
{
    public Animator animator; // 拖入角色 Animator
    public AnimatorOverrideController defaultSkin; // 拖入默认皮肤控制器
    public Button revertButton; // 拖入还原按钮

    private void Start()
    {
        revertButton.onClick.AddListener(RevertToDefaultSkin);
    }

    void RevertToDefaultSkin()
    {
        SkinManager.selectedOverrideController = defaultSkin;
        animator.runtimeAnimatorController = defaultSkin;

        // 保存选择
        PlayerPrefs.SetString("SelectedSkinName", defaultSkin.name);
        PlayerPrefs.Save();

        Debug.Log("已切换回默认皮肤：" + defaultSkin.name);
    }
}
