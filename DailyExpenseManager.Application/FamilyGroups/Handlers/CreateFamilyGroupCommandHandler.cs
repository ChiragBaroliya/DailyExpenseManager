using MediatR;
using DailyExpenseManager.Domain.Entities;
using DailyExpenseManager.Infrastructure.Mongo.Repositories;
using DailyExpenseManager.Application.FamilyGroups.Commands;

namespace DailyExpenseManager.Application.FamilyGroups.Handlers;

public class CreateFamilyGroupCommandHandler : IRequestHandler<CreateFamilyGroupCommand, string>
{
    private readonly IFamilyGroupRepository _repository;
    public CreateFamilyGroupCommandHandler(IFamilyGroupRepository repository)
    {
        _repository = repository;
    }

    public async Task<string> Handle(CreateFamilyGroupCommand request, CancellationToken cancellationToken)
    {
        var group = new FamilyGroup
        {
            Name = request.Name,
            Members = new List<FamilyMember>
            {
                new FamilyMember { UserId = request.AdminUserId, Email = request.AdminEmail, Role = UserRole.Admin }
            }
        };
        await _repository.AddAsync(group);
        return group.Id;
    }
}
