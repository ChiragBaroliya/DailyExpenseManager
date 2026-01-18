using DailyExpenseManager.Application.Authentication.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DailyExpenseManager.API.Models; // Added for APIResponse

namespace DailyExpenseManager.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/auth")]
[ApiVersion("1.0")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
    {
        try
        {
            var jwt = await _mediator.Send(command);
            return Ok(APIResponse.SuccessResponse(new { token = jwt }, "Registration successful"));
        }
        catch (Exception ex)
        {
            return BadRequest(APIResponse.ErrorResponse("Registration failed", new List<string> { ex.Message }));
        }
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
    {
        try
        {
            var jwt = await _mediator.Send(command);
            return Ok(APIResponse.SuccessResponse(new { token = jwt }, "Login successful"));
        }
        catch (Exception ex)
        {
            return BadRequest(APIResponse.ErrorResponse("Login failed", new List<string> { ex.Message }));
        }
    }
}
