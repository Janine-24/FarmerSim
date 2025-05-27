using System.Collections.Generic;
using UnityEngine;

public class Habitat : MonoBehaviour
{
    public string habitatType; // e.g., "Cow", "Sheep"
    public List<Animal> animalsInHabitat = new List<Animal>();

    public Bounds habitatBounds; // 用於限制動物移動的區域

    private void Awake()
    {
        // 計算 habitat 的範圍（可用 BoxCollider 或自訂方式）
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
}
