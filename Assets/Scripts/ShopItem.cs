using UnityEngine;

public enum ShopItemType { Animal, Feed, Habitat, Factory }


[CreateAssetMenu(menuName = "Shop/Item")]
public class ShopItem : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public int price;
    public int requiredLevel;
    public GameObject prefabToPlace;// Sprite prefab
    public ShopItemType itemType;
    public GameObject previewPrefab; //to have the shadow when drag
    public bool ignoreHabitatLayer; // use to ignore the habitat layer when place animals
    public bool isAnimal;
    public string animalType; // e.g. "Cow", "Sheep"
    public GameObject machinelevelup;
    public int requiredLevelup;

}
