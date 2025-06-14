using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Item Data", order = 50)]
public class ItemData : ScriptableObject
{
    public string itemName = "Item Name";
    public Sprite icon;
    [SerializeField] public ItemType itemType;

    [Header("Seed Info")]
    [SerializeField] public int wateringTime;
    [SerializeField] public int harvestTime ;
    [SerializeField] public Sprite seedStageSprite;
    [SerializeField] public Sprite sproutStageSprite;
    [SerializeField] public Sprite matureStageSprite;

    [Header("Harvest Info")]
    public GameObject harvestProductPrefab;
    public int harvestYield = 3;

    [Header("Tool Info")]
    [SerializeField] public int durability;
    [SerializeField] public GameObject toolActionPrefab;

    [Header("Fertilizer Info")]
    public int growthBoost; // reduce growth time

    [Header("Watering Can Info")]
    public int waterCapacity;
    public float wateringRange;

    [Header("Machine Info")]
    public bool isAutoWaterer;
    public int unlockLevel;
}

public enum ItemType
{
    Seed,
    Tool,
    Fertilizer,
    WateringCan,
    Harvest,
    Machine
}


