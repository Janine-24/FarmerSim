using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class AnimalFood : MonoBehaviour
{
    public int feedCost = 10; //price that can be adjusted 
    public float productionTime = 5f; //time to produce product can be adjuust
    public ItemData item; //the product that will be produced after feeding
    public Transform productSpawnPoint; // the point where the product will be occur
    public AudioClip feedSound; //sound
    public AnimalFeedingUI panel; //that the UI panel for feeding animals
    private bool isProducing = false;


    // export status to save data
    public AnimalData ExportData()
    {
        return new AnimalData()
        {
            animalType = GetComponent<Animal>().animalType,
            position = transform.position,
            isFed = isProducing,
            productionStartTime = isProducing ? System.DateTime.UtcNow.ToString("o") : "",
            productionDuration = isProducing ? productionTime : 0f
        };
    }

    public void LoadFromData(AnimalData data)
    {
        if (data.isFed)
        {
            isProducing = true;

            // unlock start time
            if (DateTime.TryParse(data.productionStartTime, out DateTime startTime))
            {
                float timePassed = (float)(DateTime.Now - startTime).TotalSeconds;

                if (timePassed >= data.productionDuration)
                {
                    // already completed, produce product directly
                    OnProductionComplete(); // produce product
                }
                else
                {
                    // continue production
                    float remainingTime = data.productionDuration - timePassed;
                    StartProduction(remainingTime);
                }
            }
            else
            {
                Debug.LogWarning("Failed to parse production start time.");
            }
        }
    }


    private void OnMouseDown()
    {
        panel.OpenPanel(this);
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
            panel.ShowHint("Insufficient money");
            panel.ClosePanel();
            return;
        }

        // sound and effect

        if (feedSound != null)
        {
            AudioSource.PlayClipAtPoint(feedSound, transform.position);
        }

        isProducing = true;
        panel.ClosePanel();
        panel.StartProgress(productionTime, OnProductionComplete);
    }


    private void OnProductionComplete()
    {
        isProducing = false;  //finish produce
        Debug.Log("Producing product...");
        if (item.harvestProductPrefab == null)
        {
            Debug.LogWarning("Product prefab is not assigned!");
            return;
        }
        if (productSpawnPoint == null)
        {
            Debug.LogWarning("Product spawn point is not assigned!");
            return;
        }
        GameObject product = Instantiate(item.harvestProductPrefab, transform.position, Quaternion.identity); //produce product at the animals
        Debug.Log("Product produced: " + item.itemName);

        if (product.TryGetComponent<Collectproduct>(out var cp) &&
    product.TryGetComponent<Item>(out var itemComponent))
        {
            itemComponent.data = item;
            cp.item = itemComponent;
            cp.productType = item.itemName;
        }

    }

    public bool IsProducing()
    {
        return isProducing;
    }

    public void StartProduction(float duration)
    {
        isProducing = true;
        StartCoroutine(ProduceCoroutine(duration));
    }

    private IEnumerator ProduceCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        OnProductionComplete();
    }

    //restore the status of animals
    public void RestoreProducingState()
    {
        if (!isProducing)
        {
            isProducing = true;
            panel.StartProgress(productionTime, OnProductionComplete);
        }
    }

}