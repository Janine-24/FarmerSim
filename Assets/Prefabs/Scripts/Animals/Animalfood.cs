using Unity.VisualScripting;
using UnityEngine;

public class AnimalFood : MonoBehaviour
{
    public int feedCost = 10;
    public float productionTime = 5f;
    public GameObject productPrefab;
    public Transform productSpawnPoint;
    public AudioClip feedSound;
    public ParticleSystem feedEffect;
    public AnimalFeedingUI panel;

    private bool isProducing = false;
    private void OnMouseDown()
    {
        AnimalFeedingUI.Instance.OpenPanel(GetComponent<AnimalFood>());
    }
    public void TryFeedAnimal()
    {
        if (isProducing) return;

        if (PlayerCoinManager.Instance.HasEnoughCoins(feedCost))
        {
            PlayerCoinManager.Instance.SpendCoins(feedCost);
        }
        else
        { 
            AnimalFeedingUI.Instance.ShowHint("Insufficient money");
            AnimalFeedingUI.Instance.ClosePanel();
            return;
        }

        // sound and effect
        if (feedSound != null)
            AudioSource.PlayClipAtPoint(feedSound, transform.position);

        if (feedEffect != null)
            feedEffect.Play();

        isProducing = true;
        AnimalFeedingUI.Instance.ClosePanel();
        AnimalFeedingUI.Instance.StartProgress(productionTime, OnProductionComplete);
    }


    private void OnProductionComplete()
    {
        isProducing = false;  //finish produce
        Debug.Log("Producing product...");
        Instantiate(productPrefab, productSpawnPoint.position, Quaternion.identity); // product will place at there
    }
}
