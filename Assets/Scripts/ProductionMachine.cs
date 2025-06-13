using UnityEngine;
using System.Collections;
using System;

public class ProductionMachine : MonoBehaviour
{
    public MachineRecipe recipe;
    public bool isProcessing = false;
    public float currentTimer = 0f;
    private int currentOutputAmount = 0;
    public int GetRemainingOutputs() => currentOutputAmount;
    public string machineID; // assign in inspector or on spawn

    public ProductionMachineData GetSaveData()///
    {
        return new ProductionMachineData
        {
            machineType = machineID,
            position = transform.position,
            currentRecipe = recipe?.name ?? "",
            isProducing = isProcessing,
            remainingOutputCount = currentOutputAmount,
            remainingTime = currentTimer,
            productionStartTime = DateTime.Now.ToString("o"), // 保存当前时间
            productionDuration = recipe.processingTimePerBatch * currentOutputAmount // 保存总耗时
        };
    }


    public void ResumeProcessing(ProductionMachineData data)
    {
        float passedSeconds = (float)(DateTime.Now - DateTime.Parse(data.productionStartTime)).TotalSeconds;
        float remaining = data.productionDuration - passedSeconds;

        currentOutputAmount = data.remainingOutputCount;
        currentTimer = Mathf.Max(0, remaining);
        isProcessing = true;

        StartCoroutine(ProcessRoutine());
    }

    private void Awake()
    {
        Debug.Log($"[Machine] Spawned machine: {machineID} at {transform.position}");
    }

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

    public void RestoreIdleState(string recipeID)
    {
        var recipe = Resources.Load<MachineRecipe>($"Recipes/{recipeID}");
        if (recipe == null)
        {
            Debug.LogWarning($"Could not find recipe with ID {recipeID}");
            return;
        }

        this.recipe = recipe;
        this.isProcessing = false;
        this.currentOutputAmount = 0;
        this.currentTimer = 0f;
    }

    private IEnumerator ProcessRoutine()
    {
        while (currentTimer > 0)
        {
            currentTimer -= Time.deltaTime;
            yield return null;
        }

        int totalProductCount = currentOutputAmount * recipe.outputAmountPerBatch;//1 product 10 star

        for (int i = 0; i < currentOutputAmount * recipe.outputAmountPerBatch; i++)
        {
            Vector3 offset = UnityEngine.Random.insideUnitCircle * 0.3f;
            Instantiate(recipe.outputItem.harvestProductPrefab, transform.position + offset, Quaternion.identity);
        }

        LevelSystem.Instance.AddXP(totalProductCount * 10);//add level experience

        isProcessing = false;
        Debug.Log(" Production complete.");
    }
}