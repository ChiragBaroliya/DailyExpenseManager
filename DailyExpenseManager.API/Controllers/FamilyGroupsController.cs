using MediatR;
using Microsoft.AspNetCore.Mvc;
using DailyExpenseManager.Application.FamilyGroups.Commands;
using DailyExpenseManager.API.Models;

namespace DailyExpenseManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FamilyGroupsController : ControllerBase
{
    private readonly IMediator _mediator;
    public FamilyGroupsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateFamilyGroupCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(APIResponse.SuccessResponse(new { GroupId = id }, "Family group created successfully."));
    }

    [HttpPost("invite")]
    public async Task<IActionResult> Invite([FromBody] InviteFamilyMemberCommand command)
    {
        await _mediator.Send(command);
        return Ok(APIResponse.SuccessResponse(message: "Invitation sent."));
    }

    [HttpPost("assign-role")]
    public async Task<IActionResult> AssignRole([FromBody] AssignFamilyMemberRoleCommand command)
    {
        await _mediator.Send(command);
        return Ok(APIResponse.SuccessResponse(message: "Role assigned."));
    }
}
