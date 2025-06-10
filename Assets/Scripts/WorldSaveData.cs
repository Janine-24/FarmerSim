using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WorldSaveData
{
    public List<PlantSaveData> plantedCrops = new();
    public List<MachineSaveData> activeMachines = new();
    public List<Vector3Int> hoedTiles = new();
    public long savedUnixTime; // For real-time elapsed calculation
}

[Serializable]
public class PlantSaveData
{
    public string plantName;
    public Vector3Int tilePosition;
    public string seedID;
    public int currentStage;
    public float growthTimer;
    public int currentWaterCount;
}

[Serializable]
public class MachineSaveData
{
    public string recipeID;
    public string prefabName;
    public Vector3 position;
    public float timerLeft;
    public int amountLeft;
}
