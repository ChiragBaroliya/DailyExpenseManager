using DailyExpenseManager.Domain.Entities;
using MongoDB.Driver;

namespace DailyExpenseManager.Infrastructure.Mongo.Repositories;

public interface IMonthlyBudgetRepository
{
    Task<MonthlyBudget?> GetByFamilyGroupMonthAsync(string familyGroupId, int year, int month);
    Task AddOrUpdateAsync(MonthlyBudget budget);
    Task<List<MonthlyBudget>> GetAllByFamilyGroupAsync(string familyGroupId);
}

public class MonthlyBudgetRepository : IMonthlyBudgetRepository
{
    private readonly IMongoCollection<MonthlyBudget> _budgets;
    public MonthlyBudgetRepository(IMongoDatabase db)
    {
        _budgets = db.GetCollection<MonthlyBudget>("MonthlyBudgets");
    }

    public async Task<MonthlyBudget?> GetByFamilyGroupMonthAsync(string familyGroupId, int year, int month)
        => await _budgets.Find(b => b.FamilyGroupId == familyGroupId && b.Year == year && b.Month == month).FirstOrDefaultAsync();

    public async Task AddOrUpdateAsync(MonthlyBudget budget)
    {
        var filter = Builders<MonthlyBudget>.Filter.Eq(b => b.FamilyGroupId, budget.FamilyGroupId) &
                     Builders<MonthlyBudget>.Filter.Eq(b => b.Year, budget.Year) &
                     Builders<MonthlyBudget>.Filter.Eq(b => b.Month, budget.Month);
        await _budgets.ReplaceOneAsync(filter, budget, new ReplaceOptions { IsUpsert = true });
    }

    public async Task<List<MonthlyBudget>> GetAllByFamilyGroupAsync(string familyGroupId)
        => await _budgets.Find(b => b.FamilyGroupId == familyGroupId).ToListAsync();
}
