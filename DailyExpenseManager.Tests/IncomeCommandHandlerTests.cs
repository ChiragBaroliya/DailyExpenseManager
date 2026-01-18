using Xunit;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using DailyExpenseManager.Application.Incomes.Handlers;
using DailyExpenseManager.Application.Incomes.Commands;
using DailyExpenseManager.Domain.Entities;
using DailyExpenseManager.Infrastructure.Mongo.Repositories;

public class AddIncomeCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldAddIncome()
    {
        var repo = new Mock<IIncomeRepository>();
        var command = new AddIncomeCommand(100, "Job", new DateTime(2024, 6, 1), "Salary", "user1", "group1");
        var handler = new AddIncomeCommandHandler(repo.Object);
        var result = await handler.Handle(command, CancellationToken.None);
        Assert.False(string.IsNullOrEmpty(result));
        repo.Verify(r => r.AddAsync(It.IsAny<Income>()), Times.Once);
    }
}

public class EditIncomeCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldEditIncome_WhenIncomeExists()
    {
        var repo = new Mock<IIncomeRepository>();
        var command = new EditIncomeCommand("id1", 200, "Bonus", new DateTime(2024, 6, 2), "Yearly");
        var income = new Income { Id = command.Id };
        repo.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(income);
        var handler = new EditIncomeCommandHandler(repo.Object);
        await handler.Handle(command, CancellationToken.None);
        repo.Verify(r => r.UpdateAsync(It.Is<Income>(i => i.Id == command.Id && i.Amount == command.Amount)), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenIncomeNotFound()
    {
        var repo = new Mock<IIncomeRepository>();
        var command = new EditIncomeCommand("id1", 200, "Bonus", new DateTime(2024, 6, 2), "Yearly");
        repo.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync((Income)null);
        var handler = new EditIncomeCommandHandler(repo.Object);
        await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
    }
}

public class DeleteIncomeCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldDeleteIncome()
    {
        var repo = new Mock<IIncomeRepository>();
        var command = new DeleteIncomeCommand("id1");
        var handler = new DeleteIncomeCommandHandler(repo.Object);
        await handler.Handle(command, CancellationToken.None);
        repo.Verify(r => r.DeleteAsync(command.Id), Times.Once);
    }
}
