using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MapData
{
    public List<AnimalData> animals = new();
    public List<HabitatData> habitats = new();
    public List<CloudData> clouds = new();
    public List<ProductData> products = new();
    public List<ProductionMachineData> machines = new();
    public int playerLevel = 1;
}

[Serializable]
public class AnimalData
{
    public string animalType;
    public Vector2 position;
    public bool isFed;
    public string productionStartTime;  // time starting for produce
    public float productionDuration;    // product duration time
}

[Serializable]
public class HabitatData
{
    public string habitatType;
    public Vector2 position;
}

[Serializable]
public class CloudData
{
    public Vector2 position;
    public bool isUnlocked;
}

[Serializable]
public class ProductData
{
    public Vector2 position;
    public string productType;
    public bool isCollected;
}

[System.Serializable]
public class ProductionMachineData
{
    public string machineType;
    public Vector2 position;
    public string currentRecipe;
    public float remainingTime;
    public bool isProducing;
    public int remainingOutputCount; // the number of outputs left to produce
}



