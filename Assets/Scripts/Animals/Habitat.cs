using System.Collections.Generic;
using UnityEngine;

public class Habitat : MonoBehaviour
{
    public string habitatType; // e.g., "Cow", "Sheep"
    public List<Animal> animalsInHabitat = new();
    public int maxanimals = 8;
    public Bounds habitatBounds; // limit animal movement

    private void Awake()
    {
        // calculate habitat area（use BoxCollider）
        if (TryGetComponent<Collider2D>(out var collider))
        {
            habitatBounds = collider.bounds;
        }
    }

    public void RegisterAnimal(Animal animal)
    {
        if (!animalsInHabitat.Contains(animal)) 
        {
            animalsInHabitat.Add(animal);
        }
    }
    public void UnregisterAnimal(Animal animal)
    {
        animalsInHabitat.Remove(animal);
    }

    public bool CanAcceptAnimal(Animal animal)
    {
        return animal.animalType == this.habitatType && animalsInHabitat.Count < maxanimals;
    }
    bool HasAvailableHabitatForAnimal(string animalType)
    {
        var habitats = FindObjectsByType<Habitat>(FindObjectsSortMode.None);
        foreach (var h in habitats)
        {
            if (h.habitatType == animalType && h.animalsInHabitat.Count < h.maxanimals)
                return true;
        }
        return false;
    }

}