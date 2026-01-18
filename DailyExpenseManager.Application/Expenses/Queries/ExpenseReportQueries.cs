using MediatR;
using DailyExpenseManager.Domain.Entities;
using DailyExpenseManager.Infrastructure.Mongo.Repositories;

namespace DailyExpenseManager.Application.Expenses.Queries;

public record DailyMonthlyReportQuery(string FamilyGroupId, DateTime Start, DateTime End) : IRequest<List<Expense>>;
public record CategoryWiseReportQuery(string FamilyGroupId, DateTime? Start = null, DateTime? End = null) : IRequest<List<CategoryReportItem>>;
public record MemberWiseReportQuery(string FamilyGroupId, DateTime? Start = null, DateTime? End = null) : IRequest<List<MemberReportItem>>;
