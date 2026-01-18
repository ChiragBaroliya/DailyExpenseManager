using MediatR;
using DailyExpenseManager.Domain.Entities;

namespace DailyExpenseManager.Application.Categories.Queries;

public class ListCategoriesQuery : IRequest<List<Category>>
{
    public string? FamilyGroupId { get; set; }
}
