using MediatR;
using DailyExpenseManager.Application.Expenses.Queries;
using DailyExpenseManager.Infrastructure.Mongo.Repositories;
using DailyExpenseManager.Domain.Entities;

namespace DailyExpenseManager.Application.Expenses.Handlers;

public class DailyMonthlyReportQueryHandler : IRequestHandler<DailyMonthlyReportQuery, List<Expense>>
{
    private readonly IExpenseRepository _repository;
    public DailyMonthlyReportQueryHandler(IExpenseRepository repository)
    {
        _repository = repository;
    }
    public async Task<List<Expense>> Handle(DailyMonthlyReportQuery request, CancellationToken cancellationToken)
        => await _repository.GetByFamilyGroupIdAndDateRangeAsync(request.FamilyGroupId, request.Start, request.End);
}

public class CategoryWiseReportQueryHandler : IRequestHandler<CategoryWiseReportQuery, List<CategoryReportItem>>
{
    private readonly IExpenseRepository _repository;
    public CategoryWiseReportQueryHandler(IExpenseRepository repository)
    {
        _repository = repository;
    }
    public async Task<List<CategoryReportItem>> Handle(CategoryWiseReportQuery request, CancellationToken cancellationToken)
        => await _repository.GetCategoryWiseReportAsync(request.FamilyGroupId, request.Start, request.End);
}

public class MemberWiseReportQueryHandler : IRequestHandler<MemberWiseReportQuery, List<MemberReportItem>>
{
    private readonly IExpenseRepository _repository;
    public MemberWiseReportQueryHandler(IExpenseRepository repository)
    {
        _repository = repository;
    }
    public async Task<List<MemberReportItem>> Handle(MemberWiseReportQuery request, CancellationToken cancellationToken)
        => await _repository.GetMemberWiseReportAsync(request.FamilyGroupId, request.Start, request.End);
}
