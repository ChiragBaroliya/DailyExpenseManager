using DailyExpenseManager.Domain.Entities;
using MongoDB.Driver;

namespace DailyExpenseManager.Infrastructure.Mongo.Repositories;

public interface IFamilyGroupRepository
{
    Task<FamilyGroup?> GetByIdAsync(string id);
    Task AddAsync(FamilyGroup group);
    Task UpdateAsync(FamilyGroup group);
    Task<FamilyGroup?> GetByMemberEmailAsync(string email);
}

public class FamilyGroupRepository : IFamilyGroupRepository
{
    private readonly IMongoCollection<FamilyGroup> _groups;
    public FamilyGroupRepository(IMongoDatabase db)
    {
        _groups = db.GetCollection<FamilyGroup>("FamilyGroups");
    }

    public async Task<FamilyGroup?> GetByIdAsync(string id)
        => await _groups.Find(g => g.Id == id).FirstOrDefaultAsync();

    public async Task AddAsync(FamilyGroup group)
        => await _groups.InsertOneAsync(group);

    public async Task UpdateAsync(FamilyGroup group)
        => await _groups.ReplaceOneAsync(g => g.Id == group.Id, group);

    public async Task<FamilyGroup?> GetByMemberEmailAsync(string email)
        => await _groups.Find(g => g.Members.Any(m => m.Email == email)).FirstOrDefaultAsync();
}
