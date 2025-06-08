using UnityEngine;
using System.Collections;

public class ProductionMachine : MonoBehaviour
{
    public MachineRecipe recipe;
    public bool isProcessing = false;
    public float currentTimer = 0f;
    private int currentOutputAmount = 0;

    public void Interact()
    {
        if (!isProcessing)
            MachineUIManager.Instance.OpenMachineUI(this);
    }

    public void StartProcessing(int desiredOutputCount)
    {
        if (isProcessing) return;

        currentOutputAmount = desiredOutputCount;
        currentTimer = recipe.processingTimePerBatch * desiredOutputCount;
        isProcessing = true;

        StartCoroutine(ProcessRoutine());
    }

    private IEnumerator ProcessRoutine()
    {
        while (currentTimer > 0)
        {
            currentTimer -= Time.deltaTime;
            yield return null;
        }

        for (int i = 0; i < currentOutputAmount * recipe.outputAmountPerBatch; i++)
        {
            Vector3 offset = Random.insideUnitCircle * 0.3f;
            Instantiate(recipe.outputItem.harvestProductPrefab, transform.position + offset, Quaternion.identity);
        }

        isProcessing = false;
        Debug.Log(" Production complete.");
    }
}
