using Microsoft.AspNetCore.Mvc;
using DailyExpenseManager.Application.Notifications;
using DailyExpenseManager.API.Models;

namespace DailyExpenseManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly MonthlySummaryService _summaryService;
    public NotificationController(MonthlySummaryService summaryService)
    {
        _summaryService = summaryService;
    }

    [HttpGet("monthly-summary")]
    public async Task<IActionResult> GetMonthlySummary([FromQuery] string familyGroupId, [FromQuery] int year, [FromQuery] int month)
    {
        var result = await _summaryService.GenerateMonthlySummary(familyGroupId, year, month);
        return Ok(APIResponse.SuccessResponse(result));
    }
}
