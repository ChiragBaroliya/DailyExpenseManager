using MediatR;
using DailyExpenseManager.Application.Expenses.Queries;
using DailyExpenseManager.Infrastructure.Mongo.Repositories;

namespace DailyExpenseManager.Application.Expenses.Handlers;

public class MonthlyExpenseBarChartQueryHandler : IRequestHandler<MonthlyExpenseBarChartQuery, List<MonthlyExpenseBarChartItem>>
{
    private readonly IExpenseRepository _repository;
    public MonthlyExpenseBarChartQueryHandler(IExpenseRepository repository)
    {
        _repository = repository;
    }
    public async Task<List<MonthlyExpenseBarChartItem>> Handle(MonthlyExpenseBarChartQuery request, CancellationToken cancellationToken)
        => await _repository.GetMonthlyExpenseBarChartAsync(request.FamilyGroupId, request.Year);
}

public class ExpenseTrendLineChartQueryHandler : IRequestHandler<ExpenseTrendLineChartQuery, List<ExpenseTrendLineChartItem>>
{
    private readonly IExpenseRepository _repository;
    public ExpenseTrendLineChartQueryHandler(IExpenseRepository repository)
    {
        _repository = repository;
    }
    public async Task<List<ExpenseTrendLineChartItem>> Handle(ExpenseTrendLineChartQuery request, CancellationToken cancellationToken)
        => await _repository.GetExpenseTrendLineChartAsync(request.FamilyGroupId, request.Start, request.End);
}
