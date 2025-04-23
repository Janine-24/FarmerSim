using UnityEngine;

[CreateAssetMenu(fileName = "NewProduct", menuName = "ScriptableObjects/Product")]
public class Product : ScriptableObject
{
    public string productName; // Product Name
    public int initialStock;   // Initial Stock
}
