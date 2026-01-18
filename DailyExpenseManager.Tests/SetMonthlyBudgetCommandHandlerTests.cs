using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using DailyExpenseManager.Application.Budget.Handlers;
using DailyExpenseManager.Application.Budget.Commands;
using DailyExpenseManager.Infrastructure.Mongo.Repositories;
using DailyExpenseManager.Domain.Entities;

public class SetMonthlyBudgetCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldSetMonthlyBudget()
    {
        var repo = new Mock<IMonthlyBudgetRepository>();
        var command = new SetMonthlyBudgetCommand("group1", 2024, 6, 1000);
        var handler = new SetMonthlyBudgetCommandHandler(repo.Object);
        var result = await handler.Handle(command, CancellationToken.None);
        Assert.Equal("2024-6", result);
        repo.Verify(r => r.AddOrUpdateAsync(It.Is<MonthlyBudget>(b => b.FamilyGroupId == command.FamilyGroupId && b.Amount == command.Amount)), Times.Once);
    }
}
