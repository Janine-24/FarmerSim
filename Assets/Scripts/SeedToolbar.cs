using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SeedToolbar : MonoBehaviour
{
    public List<SeedItem> availableSeeds;
    public GameObject buttonPrefab;
    public Transform buttonParent;

    public PlantingSystem plantingSystem;

    void Start()
    {
        foreach (SeedItem seed in availableSeeds)
        {
            GameObject btn = Instantiate(buttonPrefab, buttonParent);
            btn.GetComponent<Image>().sprite = seed.icon;
            btn.GetComponent<Button>().onClick.AddListener(() => SelectSeed(seed));
        }
    }

    void SelectSeed(SeedItem seed)
    {
        plantingSystem.selectedSeed = seed;
        Debug.Log("Selected seed: " + seed.seedName);
    }
}
