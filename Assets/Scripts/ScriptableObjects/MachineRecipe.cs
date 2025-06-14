using UnityEngine;

[CreateAssetMenu(menuName = "Machines/MachineRecipe")]
public class MachineRecipe : ScriptableObject
{
    public string recipeName;

    [System.Serializable]
    public class Ingredient
    {
        public ItemData item;
        public int amountPerOutput;
    }

    public Ingredient[] inputItems;

    public ItemData outputItem;
    public int outputAmountPerBatch; // typically 1
    public float processingTimePerBatch; // seconds
}
