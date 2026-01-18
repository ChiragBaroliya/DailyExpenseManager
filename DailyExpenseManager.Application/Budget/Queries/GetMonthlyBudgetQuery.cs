using MediatR;
using DailyExpenseManager.Domain.Entities;

namespace DailyExpenseManager.Application.Budget.Queries;

public record GetMonthlyBudgetQuery(string FamilyGroupId, int Year, int Month) : IRequest<MonthlyBudget?>;
