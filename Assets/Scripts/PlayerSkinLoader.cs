using UnityEngine;
//load selected skin in main scene
public class PlayerSkinLoader : MonoBehaviour
{
    void Start()//start
    {
        // Get the Animator component of the current player character.
        Animator animator = GetComponent<Animator>();

        //check skin has been selected or not
        if (SkinManager.selectedOverrideController != null)
        {
            //let selected AnimatorOverrideController use in animator
            animator.runtimeAnimatorController = SkinManager.selectedOverrideController;
            Debug.Log("Skin Use Successfully：" + SkinManager.selectedOverrideController.name);
        }
        else
        {
            Debug.Log("No skins selected,default state");
        }
    }
}