using System.Collections.Generic;
using UnityEngine;

public class Habitat : MonoBehaviour
{
    public string habitatType; // e.g., "Cow", "Sheep"
    public List<Animal> animalsInHabitat = new List<Animal>();
    public Transform productSpawnPoint;
    public Bounds habitatBounds; // limit animal movement

    private void Awake()
    {
        // calculate habitat area（use BoxCollider）
        var collider = GetComponent<Collider2D>();
        if (collider != null)
            habitatBounds = collider.bounds;
    }

    public void RegisterAnimal(Animal animal)
    {
        animalsInHabitat.Add(animal);
        animal.SetHabitat(this);
    }

    public bool CanAcceptAnimal(Animal animal)
    {
        return animal.animalType == this.habitatType;
    }
    public void SpawnProduct(GameObject productPrefab)
    {
        Vector3 center = habitatBounds.center;
        productPrefab.transform.localScale = Vector3.one * 5f;
        GameObject product = Instantiate(productPrefab, productSpawnPoint.position, Quaternion.identity);
    }
}
