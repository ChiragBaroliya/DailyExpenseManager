using DailyExpenseManager.Infrastructure.Mongo.Repositories;
using DailyExpenseManager.Domain.Entities;

namespace DailyExpenseManager.Application.Notifications;

public class MonthlySummaryService
{
    private readonly IExpenseRepository _expenseRepo;
    private readonly IMonthlyBudgetRepository _budgetRepo;
    public MonthlySummaryService(IExpenseRepository expenseRepo, IMonthlyBudgetRepository budgetRepo)
    {
        _expenseRepo = expenseRepo;
        _budgetRepo = budgetRepo;
    }

    public async Task<MonthlySummaryResult> GenerateMonthlySummary(string familyGroupId, int year, int month)
    {
        var expenses = await _expenseRepo.GetByFamilyGroupIdAndDateRangeAsync(
            familyGroupId,
            new DateTime(year, month, 1),
            new DateTime(year, month, DateTime.DaysInMonth(year, month)));
        var budget = await _budgetRepo.GetByFamilyGroupMonthAsync(familyGroupId, year, month);
        var total = expenses.Sum(e => e.Amount);
        var topCategories = expenses.GroupBy(e => e.Category)
            .Select(g => new { Category = g.Key, Amount = g.Sum(x => x.Amount) })
            .OrderByDescending(x => x.Amount)
            .Take(3)
            .ToList();
        return new MonthlySummaryResult
        {
            Year = year,
            Month = month,
            TotalExpense = total,
            Budget = budget?.Amount ?? 0,
            TopCategories = topCategories.Select(x => new CategorySummary { Category = x.Category, Amount = x.Amount }).ToList(),
            IsBudgetExceeded = budget != null && total > budget.Amount
        };
    }
}

public class MonthlySummaryResult
{
    public int Year { get; set; }
    public int Month { get; set; }
    public decimal TotalExpense { get; set; }
    public decimal Budget { get; set; }
    public List<CategorySummary> TopCategories { get; set; } = new();
    public bool IsBudgetExceeded { get; set; }
}

public class CategorySummary
{
    public string Category { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}
