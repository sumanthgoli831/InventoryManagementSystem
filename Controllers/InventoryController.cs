using InventoryManagementSystem.Models;
using InventoryManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    private readonly IoTSimulatorService _iotService;

    public InventoryController(IoTSimulatorService iotService)
    {
        _iotService = iotService;
    }

    [HttpGet]
    public ActionResult<List<InventoryItem>> GetInventory()
    {
        return Ok(_iotService.GetInventory());
    }
}