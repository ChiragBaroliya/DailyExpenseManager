using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DailyExpenseManager.Application.Expenses.Handlers;
using DailyExpenseManager.Application.Expenses.Queries;
using DailyExpenseManager.Infrastructure.Mongo.Repositories;
using DailyExpenseManager.Domain.Entities;

public class DailyMonthlyReportQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnExpenses()
    {
        var repo = new Mock<IExpenseRepository>();
        var query = new DailyMonthlyReportQuery("group1", new DateTime(2024, 6, 1), new DateTime(2024, 6, 30));
        var expenses = new List<Expense> { new Expense { Id = "e1" } };
        repo.Setup(r => r.GetByFamilyGroupIdAndDateRangeAsync(query.FamilyGroupId, query.Start, query.End)).ReturnsAsync(expenses);
        var handler = new DailyMonthlyReportQueryHandler(repo.Object);
        var result = await handler.Handle(query, CancellationToken.None);
        Assert.Equal(expenses, result);
    }
}

public class CategoryWiseReportQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnCategoryReport()
    {
        var repo = new Mock<IExpenseRepository>();
        var query = new CategoryWiseReportQuery("group1", new DateTime(2024, 6, 1), new DateTime(2024, 6, 30));
        var report = new List<CategoryReportItem> { new CategoryReportItem { Category = "Food", TotalAmount = 100 } };
        repo.Setup(r => r.GetCategoryWiseReportAsync(query.FamilyGroupId, query.Start, query.End)).ReturnsAsync(report);
        var handler = new CategoryWiseReportQueryHandler(repo.Object);
        var result = await handler.Handle(query, CancellationToken.None);
        Assert.Equal(report, result);
    }
}

public class MemberWiseReportQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnMemberReport()
    {
        var repo = new Mock<IExpenseRepository>();
        var query = new MemberWiseReportQuery("group1", new DateTime(2024, 6, 1), new DateTime(2024, 6, 30));
        var report = new List<MemberReportItem> { new MemberReportItem { MemberId = "u1", TotalAmount = 200 } };
        repo.Setup(r => r.GetMemberWiseReportAsync(query.FamilyGroupId, query.Start, query.End)).ReturnsAsync(report);
        var handler = new MemberWiseReportQueryHandler(repo.Object);
        var result = await handler.Handle(query, CancellationToken.None);
        Assert.Equal(report, result);
    }
}
