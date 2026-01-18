using DailyExpenseManager.Domain.Entities;

namespace DailyExpenseManager.Application.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
