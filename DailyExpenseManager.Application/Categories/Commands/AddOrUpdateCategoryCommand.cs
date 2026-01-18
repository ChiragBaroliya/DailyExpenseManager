using MediatR;

namespace DailyExpenseManager.Application.Categories.Commands;

public class AddOrUpdateCategoryCommand : IRequest<string>
{
    public string? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string FamilyGroupId { get; set; } = string.Empty;
    public string? Description { get; set; }
}
