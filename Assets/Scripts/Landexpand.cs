using UnityEngine;

public class Landexpand : MonoBehaviour
{
    public int unlockLevel;
    public GameObject cloudCover;

    public void TryUnlock(int playerLevel)
    {
        if (playerLevel >= unlockLevel)
            Unlock();
    }

    void Unlock()
    {
        // Play fade animation or disable cloud
        cloudCover.GetComponent<Animator>().SetTrigger("FadeOut");
        // Optionally, destroy after animation
        Destroy(cloudCover, 2f);
    }

    // call fadeout(animation) when unlocking the tile
    private Animator animator; 

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void FadeOutCloud()
    {
        animator.SetTrigger("FadeOut");
        Destroy(gameObject, 1.2f); // optional: destroy after fade
    }
}
