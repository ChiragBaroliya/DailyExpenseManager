using MediatR;
using DailyExpenseManager.Infrastructure.Mongo.Repositories;
using DailyExpenseManager.Domain.Entities;
using DailyExpenseManager.Application.FamilyGroups.Commands;

namespace DailyExpenseManager.Application.FamilyGroups.Handlers;

public class AssignFamilyMemberRoleCommandHandler : IRequestHandler<AssignFamilyMemberRoleCommand>
{
    private readonly IFamilyGroupRepository _repository;
    public AssignFamilyMemberRoleCommandHandler(IFamilyGroupRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(AssignFamilyMemberRoleCommand request, CancellationToken cancellationToken)
    {
        var group = await _repository.GetByIdAsync(request.GroupId);
        if (group == null) return;
        var member = group.Members.FirstOrDefault(m => m.UserId == request.UserId);
        if (member != null)
        {
            member.Role = request.Role;
            await _repository.UpdateAsync(group);
        }
    }
}
