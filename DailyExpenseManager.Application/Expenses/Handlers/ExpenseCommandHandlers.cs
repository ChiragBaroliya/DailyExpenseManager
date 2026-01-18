using MediatR;
using DailyExpenseManager.Domain.Entities;
using DailyExpenseManager.Application.Expenses.Commands;
using DailyExpenseManager.Infrastructure.Mongo.Repositories;

namespace DailyExpenseManager.Application.Expenses.Handlers;

public class AddExpenseCommandHandler : IRequestHandler<AddExpenseCommand, string>
{
    private readonly IExpenseRepository _repository;
    public AddExpenseCommandHandler(IExpenseRepository repository)
    {
        _repository = repository;
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
