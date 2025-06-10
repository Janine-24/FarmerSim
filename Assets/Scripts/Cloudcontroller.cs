using TMPro;
using UnityEngine;

public class CloudController : MonoBehaviour
{
    public bool isUnlocked = false;
    public GameObject cloudVisual;
    public Animator animator;
    public GameObject particleEffectPrefab;

    public void UnlockCloud()
    {
        if (isUnlocked) return;
        isUnlocked = true;
        animator.SetTrigger("FadeOutTrigger");

        if (particleEffectPrefab != null)
        {
            Instantiate(particleEffectPrefab, transform.position, Quaternion.identity);
        }

        StartCoroutine(DelayedDestroy(1.2f)); // delay destroy cloud
    }
    public CloudData ExportData()
    {
        return new CloudData()
        {
            position = transform.position,
            isUnlocked = this.isUnlocked
        };
    }

    // retore status
    public void LoadFromData(CloudData data)
    {
        isUnlocked = data.isUnlocked;
        if (isUnlocked)
            Destroy(gameObject);
    }

    private System.Collections.IEnumerator DelayedDestroy(float delay) //waiting(IEnumerator) for 1.2 seconds and destroy the object
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}