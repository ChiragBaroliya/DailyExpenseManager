using MediatR;
using DailyExpenseManager.Domain.Entities;

namespace DailyExpenseManager.Application.Incomes.Commands;

public record AddIncomeCommand(decimal Amount, string Source, DateTime Date, string Notes, string CreatedBy, string FamilyGroupId) : IRequest<string>;

public record EditIncomeCommand(string Id, decimal Amount, string Source, DateTime Date, string Notes) : IRequest;

public record DeleteIncomeCommand(string Id) : IRequest;
