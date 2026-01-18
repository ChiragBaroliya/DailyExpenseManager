using MediatR;
using DailyExpenseManager.Domain.Entities;

namespace DailyExpenseManager.Application.FamilyGroups.Commands;

public record CreateFamilyGroupCommand(string Name, string AdminUserId, string AdminEmail) : IRequest<string>;
