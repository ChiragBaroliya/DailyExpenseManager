using MediatR;
using Microsoft.AspNetCore.Mvc;
using DailyExpenseManager.Application.Expenses.Commands;
using DailyExpenseManager.Application.Incomes.Commands;
using DailyExpenseManager.API.Models;

namespace DailyExpenseManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FinanceController : ControllerBase
{
    private readonly IMediator _mediator;
    public FinanceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("expense")]
    public async Task<IActionResult> AddExpense([FromBody] AddExpenseCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(APIResponse.SuccessResponse(new { ExpenseId = id }, "Expense added successfully."));
    }

    [HttpPut("expense")]
    public async Task<IActionResult> EditExpense([FromBody] EditExpenseCommand command)
    {
        await _mediator.Send(command);
        return Ok(APIResponse.SuccessResponse(message: "Expense updated successfully."));
    }

    [HttpDelete("expense/{id}")]
    public async Task<IActionResult> DeleteExpense(string id)
    {
        await _mediator.Send(new DeleteExpenseCommand(id));
        return Ok(APIResponse.SuccessResponse(message: "Expense deleted successfully."));
    }

    [HttpPost("income")]
    public async Task<IActionResult> AddIncome([FromBody] AddIncomeCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(APIResponse.SuccessResponse(new { IncomeId = id }, "Income added successfully."));
    }

    [HttpPut("income")]
    public async Task<IActionResult> EditIncome([FromBody] EditIncomeCommand command)
    {
        await _mediator.Send(command);
        return Ok(APIResponse.SuccessResponse(message: "Income updated successfully."));
    }

    [HttpDelete("income/{id}")]
    public async Task<IActionResult> DeleteIncome(string id)
    {
        await _mediator.Send(new DeleteIncomeCommand(id));
        return Ok(APIResponse.SuccessResponse(message: "Income deleted successfully."));
    }
}
