using MediatR;
using DailyExpenseManager.Application.Budget.Commands;
using DailyExpenseManager.Domain.Entities;
using DailyExpenseManager.Infrastructure.Mongo.Repositories;

namespace DailyExpenseManager.Application.Budget.Handlers;

public class SetMonthlyBudgetCommandHandler : IRequestHandler<SetMonthlyBudgetCommand, string>
{
    private readonly IMonthlyBudgetRepository _budgetRepo;
    public SetMonthlyBudgetCommandHandler(IMonthlyBudgetRepository budgetRepo)
    {
        _budgetRepo = budgetRepo;
    }

    public async Task<string> Handle(SetMonthlyBudgetCommand request, CancellationToken cancellationToken)
    {
        var budget = new MonthlyBudget
        {
            FamilyGroupId = request.FamilyGroupId,
            Year = request.Year,
            Month = request.Month,
            Amount = request.Amount
        };
        await _budgetRepo.AddOrUpdateAsync(budget);
        return $"{request.Year}-{request.Month}";
    }
}
