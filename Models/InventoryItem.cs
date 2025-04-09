namespace InventoryManagementSystem.Models;

public class InventoryItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public DateTime LastUpdated { get; set; }
}