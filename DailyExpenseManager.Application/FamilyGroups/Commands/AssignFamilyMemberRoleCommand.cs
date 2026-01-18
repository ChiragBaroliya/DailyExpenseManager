using MediatR;
using DailyExpenseManager.Domain.Entities;

namespace DailyExpenseManager.Application.FamilyGroups.Commands;

public record AssignFamilyMemberRoleCommand(string GroupId, string UserId, UserRole Role) : IRequest;
