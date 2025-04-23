using System.Collections.Generic;
using UnityEngine;

// Manages player's inventory
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;  // Singleton instance

    [System.Serializable]
    public class ProductStock
    {
        public Product product;      // Product reference
        public int initialStock;     // Initial stock value
    }

    public List<ProductStock> allProductsWithStock = new List<ProductStock>();
    private Dictionary<Product, int> inventory = new Dictionary<Product, int>();

    [Header("Player's total inventory limit (you can change this)")]
    public int totalInventoryLimit = 100;

    // Called when the script is first loaded
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeInventory();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Initialize the inventory, loading saved values or using defaults
    private void InitializeInventory()
    {
        foreach (ProductStock productStock in allProductsWithStock)
        {
            int saved = PlayerPrefs.GetInt(GetSaveKey(productStock.product), -1);
            if (saved >= 0)
                inventory[productStock.product] = saved;
            else
                inventory[productStock.product] = productStock.initialStock;
        }
    }

    // Get how many units the player owns
    public int GetProductQuantity(Product product)
    {
        return inventory.ContainsKey(product) ? inventory[product] : 0;
    }

    // Get total number of items in player's inventory
    public int GetTotalInventoryCount()
    {
        int total = 0;
        foreach (var kvp in inventory)
        {
            total += kvp.Value;
        }
        return total;
    }

    // Add a product to inventory
    public void AddProduct(Product product, int amount)
    {
        int currentTotal = GetTotalInventoryCount();
        if (currentTotal + amount > totalInventoryLimit)
        {
            Debug.LogWarning("Not Enough Stock! No More Inventory Can Be Added!");
            return;
        }

        if (inventory.ContainsKey(product))
        {
            inventory[product] += amount;
            SaveProduct(product);
            Debug.Log($"Inventory Increases: {product.productName} +{amount}, Current Inventory: {inventory[product]}");
        }
        else
        {
            inventory[product] = amount;
            SaveProduct(product);
            Debug.Log($"New Products: {product.productName}, Current Inventory: {inventory[product]}");
        }
    }

    // Remove a product from inventory
    public bool RemoveProduct(Product product, int amount)
    {
        if (inventory.ContainsKey(product) && inventory[product] >= amount)
        {
            inventory[product] -= amount;
            SaveProduct(product);
            Debug.Log($"Inventory Decreases: {product.productName} -{amount}, Current Inventory: {inventory[product]}");
            return true;
        }

        Debug.LogWarning("Stock Not Enough, Cannot Sell!");
        return false;
    }

    // Save product quantity using PlayerPrefs
    private void SaveProduct(Product product)
    {
        PlayerPrefs.SetInt(GetSaveKey(product), inventory[product]);
        PlayerPrefs.Save();
    }

    // Get unique save key for each product
    private string GetSaveKey(Product product)
    {
        return $"product_{product.productName}";
    }
}
