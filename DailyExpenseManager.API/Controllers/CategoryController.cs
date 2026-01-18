using DailyExpenseManager.API.Models;
using DailyExpenseManager.Application.Categories.Commands;
using DailyExpenseManager.Application.Categories.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DailyExpenseManager.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/category")]
[ApiVersion("1.0")]
[Authorize]
public class CategoryController : ControllerBase
{
    private readonly IMediator _mediator;
    public CategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> AddOrUpdateCategory([FromBody] AddOrUpdateCategoryCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(APIResponse.SuccessResponse(new { CategoryId = id }, "Category added/updated successfully."));
    }

    [HttpGet]
    public async Task<IActionResult> ListCategories([FromQuery] string? familyGroupId)
    {
        var result = await _mediator.Send(new ListCategoriesQuery { FamilyGroupId = familyGroupId });
        return Ok(APIResponse.SuccessResponse(result));
    }
}
