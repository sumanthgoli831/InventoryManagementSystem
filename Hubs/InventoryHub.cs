using Microsoft.AspNetCore.SignalR;
using InventoryManagementSystem.Models; // Add this line

namespace InventoryManagementSystem.Hubs;

public class InventoryHub : Hub
{
    public async Task SendInventoryUpdate(InventoryItem item)
    {
        await Clients.All.SendAsync("ReceiveInventoryUpdate", item);
    }
}