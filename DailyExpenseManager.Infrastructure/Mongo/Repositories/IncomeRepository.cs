using DailyExpenseManager.Domain.Entities;
using MongoDB.Driver;

namespace DailyExpenseManager.Infrastructure.Mongo.Repositories;

public interface IIncomeRepository
{
    Task<Income?> GetByIdAsync(string id);
    Task AddAsync(Income income);
    Task UpdateAsync(Income income);
    Task DeleteAsync(string id);
    Task<List<Income>> GetByFamilyGroupIdAsync(string familyGroupId);
}

public class IncomeRepository : IIncomeRepository
{
    private readonly IMongoCollection<Income> _incomes;
    public IncomeRepository(IMongoDatabase db)
    {
        _incomes = db.GetCollection<Income>("Incomes");
    }

    public async Task<Income?> GetByIdAsync(string id)
        => await _incomes.Find(i => i.Id == id).FirstOrDefaultAsync();

    public async Task AddAsync(Income income)
        => await _incomes.InsertOneAsync(income);

    public async Task UpdateAsync(Income income)
        => await _incomes.ReplaceOneAsync(i => i.Id == income.Id, income);

    public async Task DeleteAsync(string id)
        => await _incomes.DeleteOneAsync(i => i.Id == id);

    public async Task<List<Income>> GetByFamilyGroupIdAsync(string familyGroupId)
        => await _incomes.Find(i => i.FamilyGroupId == familyGroupId).ToListAsync();
}
