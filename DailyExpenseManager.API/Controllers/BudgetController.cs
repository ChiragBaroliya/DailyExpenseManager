using MediatR;
using Microsoft.AspNetCore.Mvc;
using DailyExpenseManager.Application.Budget.Commands;
using DailyExpenseManager.Application.Budget.Queries;
using DailyExpenseManager.API.Models;

namespace DailyExpenseManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BudgetController : ControllerBase
{
    private readonly IMediator _mediator;
    public BudgetController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> SetMonthlyBudget([FromBody] SetMonthlyBudgetCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(APIResponse.SuccessResponse(new { BudgetId = result }, "Monthly budget set."));
    }

    [HttpGet]
    public async Task<IActionResult> GetMonthlyBudget([FromQuery] string familyGroupId, [FromQuery] int year, [FromQuery] int month)
    {
        var result = await _mediator.Send(new GetMonthlyBudgetQuery(familyGroupId, year, month));
        return Ok(APIResponse.SuccessResponse(result));
    }
}
