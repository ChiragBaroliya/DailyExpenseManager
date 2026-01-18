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
}
