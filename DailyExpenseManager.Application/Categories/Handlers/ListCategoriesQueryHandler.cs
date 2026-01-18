using MediatR;
using DailyExpenseManager.Domain.Entities;
using DailyExpenseManager.Infrastructure.Mongo.Repositories;

namespace DailyExpenseManager.Application.Categories.Handlers;

public class ListCategoriesQueryHandler : IRequestHandler<Queries.ListCategoriesQuery, List<Category>>
{
    private readonly ICategoryRepository _repo;
    public ListCategoriesQueryHandler(ICategoryRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<Category>> Handle(Queries.ListCategoriesQuery request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(request.FamilyGroupId))
            return await _repo.GetByFamilyGroupIdAsync(request.FamilyGroupId);
        return await _repo.GetAllAsync();
    }
}
