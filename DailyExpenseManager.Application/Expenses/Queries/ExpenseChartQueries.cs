using MediatR;
using DailyExpenseManager.Infrastructure.Mongo.Repositories;

namespace DailyExpenseManager.Application.Expenses.Queries;

public record MonthlyExpenseBarChartQuery(string FamilyGroupId, int Year) : IRequest<List<MonthlyExpenseBarChartItem>>;
public record ExpenseTrendLineChartQuery(string FamilyGroupId, DateTime Start, DateTime End) : IRequest<List<ExpenseTrendLineChartItem>>;
