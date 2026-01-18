using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DailyExpenseManager.API.Models;

namespace DailyExpenseManager.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/admin")]
[ApiVersion("1.0")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    [HttpGet("secure-data")]
    [Route("secure-data")]
    public IActionResult GetSecureData()
    {
        return Ok(APIResponse.SuccessResponse("This is admin-only data."));
    }
}
