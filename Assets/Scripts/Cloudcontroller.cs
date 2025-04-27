using UnityEngine;

public class CloudController : MonoBehaviour
{
    public GameObject cloudVisual;
    public Animator animator;
    public GameObject particleEffectPrefab;

    public void UnlockCloud()
    {
        animator.SetTrigger("FadeOutTrigger");

        if (particleEffectPrefab != null)
        {
            Instantiate(particleEffectPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject, 1.2f); // delay destroy cloud
    }
}
