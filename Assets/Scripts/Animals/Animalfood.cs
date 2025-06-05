using Unity.VisualScripting;
using UnityEngine;

public class AnimalFood : MonoBehaviour
{
    public int feedCost = 10; //price that can be adjusted 
    public float productionTime = 5f; //time to produce product can be adjuust
    public GameObject productPrefab; //the product that will be produced after feeding
    public Transform productSpawnPoint; // the point where the product will be occur
    public AudioClip feedSound; //sound
    public ParticleSystem feedEffect; //effect
    public AnimalFeedingUI panel; //that the UI panel for feeding animals

    private bool isProducing = false;
    private void OnMouseDown()
    {
        AnimalFeedingUI.Instance.OpenPanel(this);
    }
    public void TryFeedAnimal()
    {
        if (isProducing) return; // if already producing, do not allow to feed again

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
        {
            AudioSource.PlayClipAtPoint(feedSound, transform.position);
        }
        

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
        if (productPrefab == null)
        {
            Debug.LogWarning("Product prefab is not assigned!");
            return;
        }
        if (productSpawnPoint == null)
        {
            Debug.LogWarning("Product spawn point is not assigned!");
            return;
        }
        Instantiate(productPrefab, transform.position, Quaternion.identity); //produce product at the animals
    }
}