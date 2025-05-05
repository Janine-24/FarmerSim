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

        StartCoroutine(DelayedDestroy(1.2f)); // delay destroy cloud
    }
    private System.Collections.IEnumerator DelayedDestroy(float delay) //waiting(IEnumerator) for 1.2 seconds and destroy the object
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
