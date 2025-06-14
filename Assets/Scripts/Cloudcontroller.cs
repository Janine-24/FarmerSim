using TMPro;
using UnityEngine;

public class CloudController : MonoBehaviour
{
    public bool isUnlocked = false;
    public GameObject cloudVisual;
    public Animator animator;
    public GameObject particleEffectPrefab;
    private string cloudKey;

    private void Start()
    {
        cloudKey = "CloudUnlocked_" + gameObject.name;

        isUnlocked = PlayerPrefs.GetInt(cloudKey, 0) == 1;

        if (isUnlocked)
        {
            gameObject.SetActive(false); // disappear cloud if already unlocked
        }
    }
    public void UnlockCloud() //logic for unlock cloud
    {
        if (isUnlocked) return;
        isUnlocked = true;
        animator.SetTrigger("FadeOutTrigger");

        if (particleEffectPrefab != null)
        {
            Instantiate(particleEffectPrefab, transform.position, Quaternion.identity);
        }

        StartCoroutine(DelayedDestroy(1.2f)); // delay destroy cloud
        PlayerPrefs.SetInt(cloudKey, 1);
        PlayerPrefs.Save();
    }
    public CloudData ExportData()
    {
        return new CloudData()
        {
            position = (Vector2)transform.position,
            isUnlocked = this.isUnlocked
        };
    }

    // retore status
    public void LoadFromData(CloudData data)
    {
        transform.position = data.position;
        if (data.isUnlocked)
        {
            gameObject.SetActive(false); 
        }
        else
        {
            isUnlocked = false;
            if (cloudVisual != null)
                cloudVisual.SetActive(true);
        }
    }

    private System.Collections.IEnumerator DelayedDestroy(float delay) //waiting(IEnumerator) for 1.2 seconds and destroy the object
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}