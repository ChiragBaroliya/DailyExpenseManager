using DailyExpenseManager.Domain.Entities;
using MongoDB.Driver;

namespace DailyExpenseManager.Infrastructure.Mongo.Repositories;

public interface IExpenseRepository
{
    Task<Expense?> GetByIdAsync(string id);
    Task AddAsync(Expense expense);
    Task UpdateAsync(Expense expense);
    Task DeleteAsync(string id);
    Task<List<Expense>> GetByFamilyGroupIdAsync(string familyGroupId);
    // Reporting methods
    Task<List<Expense>> GetByFamilyGroupIdAndDateRangeAsync(string familyGroupId, DateTime start, DateTime end);
    Task<List<CategoryReportItem>> GetCategoryWiseReportAsync(string familyGroupId, DateTime? start = null, DateTime? end = null);
    Task<List<MemberReportItem>> GetMemberWiseReportAsync(string familyGroupId, DateTime? start = null, DateTime? end = null);
    // Chart methods
    Task<List<MonthlyExpenseBarChartItem>> GetMonthlyExpenseBarChartAsync(string familyGroupId, int year);
    Task<List<ExpenseTrendLineChartItem>> GetExpenseTrendLineChartAsync(string familyGroupId, DateTime start, DateTime end);
}

public class CategoryReportItem
{
    public string Category { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
}

public class MemberReportItem
{
    public string MemberId { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
}

public class MonthlyExpenseBarChartItem
{
    public int Month { get; set; }
    public decimal TotalAmount { get; set; }
}

public class ExpenseTrendLineChartItem
{
    public DateTime Date { get; set; }
    public decimal TotalAmount { get; set; }
}

public class ExpenseRepository : IExpenseRepository
{
    private readonly IMongoCollection<Expense> _expenses;
    public ExpenseRepository(IMongoDatabase db)
    {
        _expenses = db.GetCollection<Expense>("Expenses");
    }

    public async Task<Expense?> GetByIdAsync(string id)
        => await _expenses.Find(e => e.Id == id).FirstOrDefaultAsync();

    public async Task AddAsync(Expense expense)
        => await _expenses.InsertOneAsync(expense);

    public async Task UpdateAsync(Expense expense)
        => await _expenses.ReplaceOneAsync(e => e.Id == expense.Id, expense);

    public async Task DeleteAsync(string id)
        => await _expenses.DeleteOneAsync(e => e.Id == id);

    public async Task<List<Expense>> GetByFamilyGroupIdAsync(string familyGroupId)
        => await _expenses.Find(e => e.FamilyGroupId == familyGroupId).ToListAsync();

    public async Task<List<Expense>> GetByFamilyGroupIdAndDateRangeAsync(string familyGroupId, DateTime start, DateTime end)
        => await _expenses.Find(e => e.FamilyGroupId == familyGroupId && e.Date >= start && e.Date <= end).ToListAsync();

    public async Task<List<CategoryReportItem>> GetCategoryWiseReportAsync(string familyGroupId, DateTime? start = null, DateTime? end = null)
    {
        var filter = Builders<Expense>.Filter.Eq(e => e.FamilyGroupId, familyGroupId);
        if (start.HasValue && end.HasValue)
        {
            filter &= Builders<Expense>.Filter.Gte(e => e.Date, start.Value) & Builders<Expense>.Filter.Lte(e => e.Date, end.Value);
        }
        var group = await _expenses.Aggregate()
            .Match(filter)
            .Group(e => e.Category, g => new CategoryReportItem { Category = g.Key, TotalAmount = g.Sum(x => x.Amount) })
            .ToListAsync();
        return group;
    }

    public async Task<List<MemberReportItem>> GetMemberWiseReportAsync(string familyGroupId, DateTime? start = null, DateTime? end = null)
    {
        var filter = Builders<Expense>.Filter.Eq(e => e.FamilyGroupId, familyGroupId);
        if (start.HasValue && end.HasValue)
        {
            filter &= Builders<Expense>.Filter.Gte(e => e.Date, start.Value) & Builders<Expense>.Filter.Lte(e => e.Date, end.Value);
        }
        var group = await _expenses.Aggregate()
            .Match(filter)
            .Group(e => e.CreatedBy, g => new MemberReportItem { MemberId = g.Key, TotalAmount = g.Sum(x => x.Amount) })
            .ToListAsync();
        return group;
    }

    public async Task<List<MonthlyExpenseBarChartItem>> GetMonthlyExpenseBarChartAsync(string familyGroupId, int year)
    {
        var filter = Builders<Expense>.Filter.Eq(e => e.FamilyGroupId, familyGroupId) &
                     Builders<Expense>.Filter.Gte(e => e.Date, new DateTime(year, 1, 1)) &
                     Builders<Expense>.Filter.Lte(e => e.Date, new DateTime(year, 12, 31));
        var group = await _expenses.Aggregate()
            .Match(filter)
            .Group(e => e.Date.Month, g => new MonthlyExpenseBarChartItem { Month = g.Key, TotalAmount = g.Sum(x => x.Amount) })
            .SortBy(x => x.Month)
            .ToListAsync();
        return group;
    }

    public async Task<List<ExpenseTrendLineChartItem>> GetExpenseTrendLineChartAsync(string familyGroupId, DateTime start, DateTime end)
    {
        var filter = Builders<Expense>.Filter.Eq(e => e.FamilyGroupId, familyGroupId) &
                     Builders<Expense>.Filter.Gte(e => e.Date, start) &
                     Builders<Expense>.Filter.Lte(e => e.Date, end);
        var group = await _expenses.Aggregate()
            .Match(filter)
            .Group(e => e.Date, g => new ExpenseTrendLineChartItem { Date = g.Key, TotalAmount = g.Sum(x => x.Amount) })
            .SortBy(x => x.Date)
            .ToListAsync();
        return group;
    }
}
