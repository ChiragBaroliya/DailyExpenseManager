using MediatR;
using DailyExpenseManager.Infrastructure.Mongo.Repositories;
using DailyExpenseManager.Application.FamilyGroups.Commands;

namespace DailyExpenseManager.Application.FamilyGroups.Handlers;

public class InviteFamilyMemberCommandHandler : IRequestHandler<InviteFamilyMemberCommand>
{
    private readonly IFamilyGroupRepository _repository;
    public InviteFamilyMemberCommandHandler(IFamilyGroupRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(InviteFamilyMemberCommand request, CancellationToken cancellationToken)
    {
        var group = await _repository.GetByIdAsync(request.GroupId);
        if (group == null) return;
        if (!group.PendingInvitations.Contains(request.Email))
            group.PendingInvitations.Add(request.Email);
        await _repository.UpdateAsync(group);
    }
}
