using System.Collections.Generic;
using UnityEngine;

// Manager for the selling machine
public class SellingMachineManager : MonoBehaviour
{
    [System.Serializable]
    public class MachineStock
    {
        public Product product;  // The product
        public int stock;        // The quantity in stock
    }

    public List<MachineStock> machineStock = new List<MachineStock>();  // List of all stocks

    // Method to sell a product
    public bool SellProduct(Product product, int amount)
    {
        var stockEntry = machineStock.Find(entry => entry.product == product);
        if (stockEntry != null && stockEntry.stock >= amount)
        {
            // Check if the player has enough product in their inventory
            if (InventoryManager.Instance.RemoveProduct(product, amount))
            {
                stockEntry.stock -= amount;  // Decrease machine stock
                Debug.Log($"Successfully Sold {amount} {product.productName}. Selling Machine Remaining Stock: {stockEntry.stock}");
                return true;
            }
            else
            {
                Debug.Log("Inventory is insufficient and cannot be sold!");
            }
        }
        else
        {
            Debug.Log("Selling Machine Out Of Stock!");
        }
        return false;
    }

    // Get current stock of a specific product
    public int GetStock(Product product)
    {
        var stockEntry = machineStock.Find(entry => entry.product == product);
        return stockEntry != null ? stockEntry.stock : 0;
    }

    // Set stock to a specific value
    public void SetStock(Product product, int amount)
    {
        var stockEntry = machineStock.Find(entry => entry.product == product);
        if (stockEntry != null) stockEntry.stock = amount;
    }

    // Add stock to the machine
    public void AddStock(Product product, int amount)
    {
        var stockEntry = machineStock.Find(entry => entry.product == product);
        if (stockEntry != null)
            stockEntry.stock += amount;
        else
            machineStock.Add(new MachineStock { product = product, stock = amount });
    }
}
