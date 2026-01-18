using MediatR;
using DailyExpenseManager.Domain.Entities;
using DailyExpenseManager.Infrastructure.Mongo.Repositories;

namespace DailyExpenseManager.Application.Categories.Handlers;

public class AddOrUpdateCategoryCommandHandler : IRequestHandler<Commands.AddOrUpdateCategoryCommand, string>
{
    private readonly ICategoryRepository _repo;
    public AddOrUpdateCategoryCommandHandler(ICategoryRepository repo)
    {
        _repo = repo;
    }

    public async Task<string> Handle(Commands.AddOrUpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new Category
        {
            Id = string.IsNullOrEmpty(request.Id) ? Guid.NewGuid().ToString() : request.Id,
            Name = request.Name,
            FamilyGroupId = request.FamilyGroupId,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow
        };
        await _repo.AddOrUpdateAsync(category);
        return category.Id;
    }
}
