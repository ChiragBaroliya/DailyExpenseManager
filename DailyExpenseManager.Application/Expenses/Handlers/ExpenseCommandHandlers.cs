using MediatR;
using DailyExpenseManager.Domain.Entities;
using DailyExpenseManager.Application.Expenses.Commands;
using DailyExpenseManager.Infrastructure.Mongo.Repositories;
using DailyExpenseManager.Domain.Entities;

namespace DailyExpenseManager.Application.Expenses.Handlers;

public class AddExpenseCommandHandler : IRequestHandler<AddExpenseCommand, string>
{
    private readonly IExpenseRepository _repository;
    private readonly IMonthlyBudgetRepository _budgetRepo;
    public AddExpenseCommandHandler(IExpenseRepository repository, IMonthlyBudgetRepository budgetRepo)
    {
        _repository = repository;
        _budgetRepo = budgetRepo;
    }
    public async Task<string> Handle(AddExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = new Expense
        {
            Id = Guid.NewGuid().ToString(),
            Amount = request.Amount,
            Category = request.Category,
            PaymentMode = request.PaymentMode,
            Date = request.Date,
            Notes = request.Notes,
            CreatedBy = request.CreatedBy,
            FamilyGroupId = request.FamilyGroupId
        };
        // Budget check logic
        var budget = await _budgetRepo.GetByFamilyGroupMonthAsync(request.FamilyGroupId, request.Date.Year, request.Date.Month);
        if (budget != null)
        {
            var expenses = await _repository.GetByFamilyGroupIdAndDateRangeAsync(request.FamilyGroupId,
                new DateTime(request.Date.Year, request.Date.Month, 1),
                new DateTime(request.Date.Year, request.Date.Month, DateTime.DaysInMonth(request.Date.Year, request.Date.Month)));
            var total = expenses.Sum(e => e.Amount) + request.Amount;
            if (total > budget.Amount)
            {
                // Alert: budget exceeded
                // You can trigger notification logic here or return a special response
                // For now, just throw exception
                throw new Exception("Monthly budget exceeded!");
            }
        }
        await _repository.AddAsync(expense);
        return expense.Id;
    }
}

public class EditExpenseCommandHandler : IRequestHandler<EditExpenseCommand>
{
    private readonly IExpenseRepository _repository;
    public EditExpenseCommandHandler(IExpenseRepository repository)
    {
        _repository = repository;
    }
    public async Task Handle(EditExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = await _repository.GetByIdAsync(request.Id);
        if (expense == null) throw new Exception("Expense not found");
        expense.Amount = request.Amount;
        expense.Category = request.Category;
        expense.PaymentMode = request.PaymentMode;
        expense.Date = request.Date;
        expense.Notes = request.Notes;
        await _repository.UpdateAsync(expense);
    }
}

public class DeleteExpenseCommandHandler : IRequestHandler<DeleteExpenseCommand>
{
    private readonly IExpenseRepository _repository;
    public DeleteExpenseCommandHandler(IExpenseRepository repository)
    {
        _repository = repository;
    }
    public async Task Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.Id);
    }
}
