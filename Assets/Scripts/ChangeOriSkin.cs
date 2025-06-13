using UnityEngine;
using UnityEngine.UI;

public class ChangeOriSkin : MonoBehaviour
{
    public Animator animator; // Drag in the character Animator
    public AnimatorOverrideController defaultSkin; // Drag in the default skin controller
    public Button revertButton; 

    private void Start()
    {
        revertButton.onClick.AddListener(RevertToDefaultSkin);
    }

    void RevertToDefaultSkin()
    {
        SkinManager.selectedOverrideController = defaultSkin;
        animator.runtimeAnimatorController = defaultSkin;

        
        PlayerPrefs.SetString("SelectedSkinName", defaultSkin.name);//save selected skin
        PlayerPrefs.Save();

        Debug.Log("Already change to default skin：" + defaultSkin.name);
    }
}
