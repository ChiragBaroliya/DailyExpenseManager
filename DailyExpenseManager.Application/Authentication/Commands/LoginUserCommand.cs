using DailyExpenseManager.Application.CQRS;

namespace DailyExpenseManager.Application.Authentication.Commands;

public class LoginUserCommand : ICommand<string> // Returns JWT
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
