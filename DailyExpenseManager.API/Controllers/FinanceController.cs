using MediatR;
using Microsoft.AspNetCore.Mvc;
using DailyExpenseManager.Application.Expenses.Commands;
using DailyExpenseManager.Application.Incomes.Commands;
using DailyExpenseManager.API.Models;
using DailyExpenseManager.Application.Expenses.Queries;
using DailyExpenseManager.Infrastructure.Mongo.Repositories;

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

    // --- Reports ---
    [HttpGet("report/daily-monthly")]
    public async Task<IActionResult> GetDailyMonthlyReport([FromQuery] string familyGroupId, [FromQuery] DateTime start, [FromQuery] DateTime end)
    {
        var result = await _mediator.Send(new DailyMonthlyReportQuery(familyGroupId, start, end));
        return Ok(APIResponse.SuccessResponse(result));
    }

    [HttpGet("report/category-wise")]
    public async Task<IActionResult> GetCategoryWiseReport([FromQuery] string familyGroupId, [FromQuery] DateTime? start, [FromQuery] DateTime? end)
    {
        var result = await _mediator.Send(new CategoryWiseReportQuery(familyGroupId, start, end));
        return Ok(APIResponse.SuccessResponse(result));
    }

    [HttpGet("report/member-wise")]
    public async Task<IActionResult> GetMemberWiseReport([FromQuery] string familyGroupId, [FromQuery] DateTime? start, [FromQuery] DateTime? end)
    {
        var result = await _mediator.Send(new MemberWiseReportQuery(familyGroupId, start, end));
        return Ok(APIResponse.SuccessResponse(result));
    }

    // --- Charts ---
    [HttpGet("chart/pie-category")]
    public async Task<IActionResult> GetPieCategoryChart([FromQuery] string familyGroupId, [FromQuery] DateTime? start, [FromQuery] DateTime? end)
    {
        var result = await _mediator.Send(new CategoryWiseReportQuery(familyGroupId, start, end));
        return Ok(APIResponse.SuccessResponse(result));
    }

    [HttpGet("chart/bar-monthly")]
    public async Task<IActionResult> GetBarMonthlyChart([FromQuery] string familyGroupId, [FromQuery] int year)
    {
        var result = await _mediator.Send(new MonthlyExpenseBarChartQuery(familyGroupId, year));
        return Ok(APIResponse.SuccessResponse(result));
    }

    [HttpGet("chart/line-trend")]
    public async Task<IActionResult> GetLineTrendChart([FromQuery] string familyGroupId, [FromQuery] DateTime start, [FromQuery] DateTime end)
    {
        var result = await _mediator.Send(new ExpenseTrendLineChartQuery(familyGroupId, start, end));
        return Ok(APIResponse.SuccessResponse(result));
    }
}
