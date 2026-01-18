using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using DailyExpenseManager.Application.Budget.Handlers;
using DailyExpenseManager.Application.Budget.Queries;
using DailyExpenseManager.Infrastructure.Mongo.Repositories;
using DailyExpenseManager.Domain.Entities;

public class GetMonthlyBudgetQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnBudget_WhenExists()
    {
        var repo = new Mock<IMonthlyBudgetRepository>();
        var query = new GetMonthlyBudgetQuery("group1", 2024, 6);
        var budget = new MonthlyBudget { FamilyGroupId = query.FamilyGroupId, Year = query.Year, Month = query.Month, Amount = 1000 };
        repo.Setup(r => r.GetByFamilyGroupMonthAsync(query.FamilyGroupId, query.Year, query.Month)).ReturnsAsync(budget);
        var handler = new GetMonthlyBudgetQueryHandler(repo.Object);
        var result = await handler.Handle(query, CancellationToken.None);
        Assert.Equal(budget, result);
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenNotExists()
    {
        var repo = new Mock<IMonthlyBudgetRepository>();
        var query = new GetMonthlyBudgetQuery("group1", 2024, 6);
        repo.Setup(r => r.GetByFamilyGroupMonthAsync(query.FamilyGroupId, query.Year, query.Month)).ReturnsAsync((MonthlyBudget)null);
        var handler = new GetMonthlyBudgetQueryHandler(repo.Object);
        var result = await handler.Handle(query, CancellationToken.None);
        Assert.Null(result);
    }
}
