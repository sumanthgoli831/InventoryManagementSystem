using InventoryManagementSystem.Hubs;
using InventoryManagementSystem.Models;
using Microsoft.AspNetCore.SignalR;

namespace InventoryManagementSystem.Services;

public class IoTSimulatorService : BackgroundService
{
    private readonly IHubContext<InventoryHub> _hubContext;
    private readonly List<InventoryItem> _inventory;

    public IoTSimulatorService(IHubContext<InventoryHub> hubContext)
    {
        _hubContext = hubContext;
        _inventory = new List<InventoryItem>
        {
            new InventoryItem { Id = 1, Name = "Laptop", Quantity = 10, LastUpdated = DateTime.UtcNow },
            new InventoryItem { Id = 2, Name = "Mouse", Quantity = 50, LastUpdated = DateTime.UtcNow }
        };
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // Simulate IoT device updating stock
            var random = new Random();
            var item = _inventory[random.Next(_inventory.Count)];
            item.Quantity += random.Next(-5, 6); // Random change between -5 and +5
            item.LastUpdated = DateTime.UtcNow;

            // Broadcast update via SignalR
            await _hubContext.Clients.All.SendAsync("ReceiveInventoryUpdate", item);
            await Task.Delay(5000, stoppingToken); // Update every 5 seconds
        }
    }

    public List<InventoryItem> GetInventory() => _inventory;
}