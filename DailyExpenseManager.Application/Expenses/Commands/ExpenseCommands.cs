using MediatR;
using DailyExpenseManager.Domain.Entities;

namespace DailyExpenseManager.Application.Expenses.Commands;

public record AddExpenseCommand(decimal Amount, string Category, string PaymentMode, DateTime Date, string Notes, string CreatedBy, string FamilyGroupId) : IRequest<string>;

public record EditExpenseCommand(string Id, decimal Amount, string Category, string PaymentMode, DateTime Date, string Notes) : IRequest;

public record DeleteExpenseCommand(string Id) : IRequest;
