using Microsoft.AspNetCore.Mvc;
using WebApplication1.Exceptions;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/washing-machines")]
public class WashingMachinesController : ControllerBase
{
    private readonly DbService _service;

    public WashingMachinesController(DbService service)
    {
        _service = service;
    }


    [HttpGet("{customerId}/purchases")]
    public async Task<IActionResult> GetCustomerPurchases(int customerId)
    {
        var result = await _service.GetCustomerPurchasesAsync(customerId);

        if (result == null)
            return NotFound();

        return Ok(result);

    }


    [HttpPost]
        public async Task<IActionResult> AddWashingMachine([FromBody] CreateWashingMachineRequest request)
        {
            try
            {
                await _service.AddWashingMachineAsync(request);
                return Ok(new { message = "Washing machine added successfully." });
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (ConflictException ex)
            {
                return Conflict(new { error = ex.Message });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Unexpected server error." });
            }
        }
    
}