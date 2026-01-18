using DailyExpenseManager.Domain.Entities;
using MongoDB.Driver;

namespace DailyExpenseManager.Infrastructure.Mongo.Repositories;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(string id);
    Task AddOrUpdateAsync(Category category);
    Task<List<Category>> GetByFamilyGroupIdAsync(string familyGroupId);
    Task<List<Category>> GetAllAsync();
}

public class CategoryRepository : ICategoryRepository
{
    private readonly IMongoCollection<Category> _categories;
    public CategoryRepository(IMongoDatabase db)
    {
        _categories = db.GetCollection<Category>("Categories");
    }

    public async Task<Category?> GetByIdAsync(string id)
        => await _categories.Find(c => c.Id == id).FirstOrDefaultAsync();

    public async Task AddOrUpdateAsync(Category category)
    {
        var existing = await GetByIdAsync(category.Id);
        if (existing == null)
            await _categories.InsertOneAsync(category);
        else
            await _categories.ReplaceOneAsync(c => c.Id == category.Id, category);
    }

    public async Task<List<Category>> GetByFamilyGroupIdAsync(string familyGroupId)
        => await _categories.Find(c => c.FamilyGroupId == familyGroupId).ToListAsync();

    public async Task<List<Category>> GetAllAsync()
        => await _categories.Find(_ => true).ToListAsync();
}
