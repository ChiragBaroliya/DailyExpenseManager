using MediatR;

namespace DailyExpenseManager.Application.FamilyGroups.Commands;

public record InviteFamilyMemberCommand(string GroupId, string Email) : IRequest;
