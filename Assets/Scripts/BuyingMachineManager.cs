using System.Collections.Generic;
using UnityEngine;

// Manager for the buying machine
public class BuyingMachineManager : MonoBehaviour
{
    [System.Serializable]
    public class MachineStock
    {
        public Product product;  // The product
        public int stock;        // Available quantity
    }

    public List<MachineStock> machineStock = new List<MachineStock>();  // List of stock entries

    // Player buys a product from the machine
    public bool BuyProduct(Product product, int amount)
    {
        var stockEntry = machineStock.Find(entry => entry.product == product);
        if (stockEntry != null && stockEntry.stock >= amount)
        {
            stockEntry.stock -= amount;  // Decrease machine stock
            InventoryManager.Instance.AddProduct(product, amount);  // Add to player's inventory
            Debug.Log($"Successfully Buy {amount} {product.productName}. Buying Machine Remaining Stock: {stockEntry.stock}");
            return true;
        }

        Debug.Log($"Failed To Buy: {product.productName} Stock Not Enough!");
        return false;
    }

    // Refresh the machine stock daily with random amounts
    public void RefreshDailyStock()
    {
        foreach (var stockEntry in machineStock)
        {
            stockEntry.stock = Random.Range(1, 6);  // Random stock from 1 to 5
        }
        Debug.Log("Buying machine inventory has been refreshed!");
    }

    // Get current stock of a product
    public int GetStock(Product product)
    {
        var stockEntry = machineStock.Find(entry => entry.product == product);
        return stockEntry != null ? stockEntry.stock : 0;
    }

    // Set a specific stock value
    public void SetStock(Product product, int amount)
    {
        var stockEntry = machineStock.Find(entry => entry.product == product);
        if (stockEntry != null) stockEntry.stock = amount;
    }

    // Add to stock
    public void AddStock(Product product, int amount)
    {
        var stockEntry = machineStock.Find(entry => entry.product == product);
        if (stockEntry != null)
            stockEntry.stock += amount;
        else
            machineStock.Add(new MachineStock { product = product, stock = amount });
    }
}
