using UnityEngine;

[CreateAssetMenu(fileName = "NewSeed", menuName = "Farming/Seed")]
public class SeedItem : ScriptableObject
{
    public string seedName;
    public Sprite icon;
    public GameObject cropPrefab;
}
