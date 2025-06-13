using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class WorldSaveManager
{
    private static string SavePath => Application.persistentDataPath + "/worldsave.json";

    public static void SaveWorld()
    {

        if (GameManager.instance == null)
        {
            Debug.LogWarning("❌ GameManager.instance is null during save.");
            return;
        }

        if (GameManager.instance.tileManager == null)
        {
            Debug.LogWarning("❌ tileManager is null during save.");
            return;
        }

        if (GameManager.instance.itemManager == null)
        {
            Debug.LogWarning("❌ itemManager is null during save.");
            return;
        }
        WorldSaveData data = new();
        data.savedUnixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        foreach (var plant in GameManager.instance.tileManager.plantedCrops)
        {
            if (plant == null) continue;

            PlantSaveData pd = new()
            {
                plantName = plant.plantName,
                tilePosition = plant.tilePosition,
                seedID = plant.seedData.itemName,
                currentStage = plant.CurrentStage,
                growthTimer = plant.GetGrowthTimer(),
                currentWaterCount = plant.WaterCount
            };

            data.plantedCrops.Add(pd);
        }

        foreach (var machine in UnityEngine.Object.FindObjectsOfType<ProductionMachine>())
        {
            if (string.IsNullOrEmpty(machine.machineID))
                Debug.LogWarning($"❌ Machine at {machine.transform.position} is missing machineID!");
            if (string.IsNullOrEmpty(machine.machineID))
            {
                Debug.LogWarning($"❌ Skipping machine at {machine.transform.position} — machineID is missing");
                continue;
            }

            if (machine.recipe == null)
            {
                Debug.LogWarning($"❌ Skipping machine {machine.machineID} — recipe is missing");
                continue;
            }
            Debug.Log($"🧪 Saving machine: ID={machine.machineID}, Pos={machine.transform.position}");
            data.activeMachines.Add(new MachineSaveData
            {
                recipeID = machine.recipe.recipeName,
                prefabName = machine.machineID,
                position = machine.transform.position,
                timerLeft = machine.currentTimer,
                amountLeft = machine.GetRemainingOutputs()
            });
        }


        foreach (var tile in GameManager.instance.tileManager.hoedTiles)
        {
            data.hoedTiles.Add(tile);
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
        Debug.Log("✅ World saved.");
    }

    public static void LoadWorld()
    {
        if (!File.Exists(SavePath)) return;

        string json = File.ReadAllText(SavePath);
        WorldSaveData data = JsonUtility.FromJson<WorldSaveData>(json);
        long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        float elapsed = now - data.savedUnixTime;
        //reapplies hoed/soil visual and tracking
        foreach (var tile in data.hoedTiles)
        {
            GameManager.instance.tileManager.SetInteracted(tile);
        }


        // Restore plants
        foreach (var saved in data.plantedCrops)
        {
            ItemData seed = GameManager.instance.itemManager.GetItemByName(saved.seedID)?.data;
            if (seed == null) continue;

            GameManager.instance.tileManager.RestorePlantedSeed(saved.tilePosition, seed, saved.currentStage, saved.growthTimer, elapsed, saved.currentWaterCount);

        }

        // Restore machines
        foreach (var saved in data.activeMachines)
        {
            GameObject prefab = Resources.Load<GameObject>($"Machines/{saved.prefabName}");
            if (prefab == null)
            {
                Debug.LogWarning($"❌ Missing prefab: Machines/{saved.prefabName}");
                continue;
            }


            GameObject machineObj = GameObject.Instantiate(prefab, saved.position, Quaternion.identity);
            var machine = machineObj.GetComponent<ProductionMachine>();
            if (machine == null) continue;

            // Resume if was processing, or restore as idle
            if (saved.amountLeft > 0)
            {
                machine.ResumeProcessing(new ProductionMachineData
                {
                    remainingOutputCount = machine.GetRemainingOutputs(),
                    remainingTime = machine.currentTimer,

                });
            }
            else
            {
                machine.RestoreIdleState(saved.recipeID); // <--- You need to implement this
            }
        }

    }

    private static ProductionMachine FindMachineAt(Vector3 pos)
    {
        foreach (var m in UnityEngine.Object.FindObjectsOfType<ProductionMachine>())
        {
            if (Vector3.Distance(m.transform.position, pos) < 0.1f)
                return m;
        }
        return null;
    }
}
