using MediatR;

namespace DailyExpenseManager.Application.Budget.Commands;

public record SetMonthlyBudgetCommand(string FamilyGroupId, int Year, int Month, decimal Amount) : IRequest<string>;
