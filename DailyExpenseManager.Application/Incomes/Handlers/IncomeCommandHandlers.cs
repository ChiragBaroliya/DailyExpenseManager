using MediatR;
using DailyExpenseManager.Domain.Entities;
using DailyExpenseManager.Application.Incomes.Commands;
using DailyExpenseManager.Infrastructure.Mongo.Repositories;

namespace DailyExpenseManager.Application.Incomes.Handlers;

public class AddIncomeCommandHandler : IRequestHandler<AddIncomeCommand, string>
{
    private readonly IIncomeRepository _repository;
    public AddIncomeCommandHandler(IIncomeRepository repository)
    {
        _repository = repository;
    }
    public async Task<string> Handle(AddIncomeCommand request, CancellationToken cancellationToken)
    {
        var income = new Income
        {
            Id = Guid.NewGuid().ToString(),
            Amount = request.Amount,
            Source = request.Source,
            Date = request.Date,
            Notes = request.Notes,
            CreatedBy = request.CreatedBy,
            FamilyGroupId = request.FamilyGroupId
        };
        await _repository.AddAsync(income);
        return income.Id;
    }
}

public class EditIncomeCommandHandler : IRequestHandler<EditIncomeCommand>
{
    private readonly IIncomeRepository _repository;
    public EditIncomeCommandHandler(IIncomeRepository repository)
    {
        _repository = repository;
    }
    public async Task Handle(EditIncomeCommand request, CancellationToken cancellationToken)
    {
        var income = await _repository.GetByIdAsync(request.Id);
        if (income == null) throw new Exception("Income not found");
        income.Amount = request.Amount;
        income.Source = request.Source;
        income.Date = request.Date;
        income.Notes = request.Notes;
        await _repository.UpdateAsync(income);
    }
}

public class DeleteIncomeCommandHandler : IRequestHandler<DeleteIncomeCommand>
{
    private readonly IIncomeRepository _repository;
    public DeleteIncomeCommandHandler(IIncomeRepository repository)
    {
        _repository = repository;
    }
    public async Task Handle(DeleteIncomeCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.Id);
    }
}
