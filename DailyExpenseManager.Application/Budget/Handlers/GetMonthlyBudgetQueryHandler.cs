using MediatR;
using DailyExpenseManager.Application.Budget.Queries;
using DailyExpenseManager.Domain.Entities;
using DailyExpenseManager.Infrastructure.Mongo.Repositories;

namespace DailyExpenseManager.Application.Budget.Handlers;

public class GetMonthlyBudgetQueryHandler : IRequestHandler<GetMonthlyBudgetQuery, MonthlyBudget?>
{
    private readonly IMonthlyBudgetRepository _budgetRepo;
    public GetMonthlyBudgetQueryHandler(IMonthlyBudgetRepository budgetRepo)
    {
        _budgetRepo = budgetRepo;
    }

    public async Task<MonthlyBudget?> Handle(GetMonthlyBudgetQuery request, CancellationToken cancellationToken)
    {
        return await _budgetRepo.GetByFamilyGroupMonthAsync(request.FamilyGroupId, request.Year, request.Month);
    }
}
