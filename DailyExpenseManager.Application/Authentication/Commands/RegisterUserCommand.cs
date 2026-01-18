using DailyExpenseManager.Application.CQRS;

namespace DailyExpenseManager.Application.Authentication.Commands;

public class RegisterUserCommand : ICommand<string> // Returns JWT
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
}
