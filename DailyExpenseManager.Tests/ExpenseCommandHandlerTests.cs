using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DailyExpenseManager.Application.Expenses.Handlers;
using DailyExpenseManager.Application.Expenses.Commands;
using DailyExpenseManager.Domain.Entities;
using DailyExpenseManager.Infrastructure.Mongo.Repositories;

public class AddExpenseCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldAddExpense_WhenBudgetNotExceeded()
    {
        var expenseRepo = new Mock<IExpenseRepository>();
        var budgetRepo = new Mock<IMonthlyBudgetRepository>();
        var command = new AddExpenseCommand(100, "Food", "Cash", new DateTime(2024, 6, 1), "Lunch", "user1", "group1");
        budgetRepo.Setup(r => r.GetByFamilyGroupMonthAsync(command.FamilyGroupId, command.Date.Year, command.Date.Month)).ReturnsAsync(new MonthlyBudget { Amount = 500 });
        expenseRepo.Setup(r => r.GetByFamilyGroupIdAndDateRangeAsync(command.FamilyGroupId, It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(new List<Expense>());
        var handler = new AddExpenseCommandHandler(expenseRepo.Object, budgetRepo.Object);
        var result = await handler.Handle(command, CancellationToken.None);
        Assert.False(string.IsNullOrEmpty(result));
        expenseRepo.Verify(r => r.AddAsync(It.IsAny<Expense>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenBudgetExceeded()
    {
        var expenseRepo = new Mock<IExpenseRepository>();
        var budgetRepo = new Mock<IMonthlyBudgetRepository>();
        var command = new AddExpenseCommand(600, "Food", "Cash", new DateTime(2024, 6, 1), "Lunch", "user1", "group1");
        budgetRepo.Setup(r => r.GetByFamilyGroupMonthAsync(command.FamilyGroupId, command.Date.Year, command.Date.Month)).ReturnsAsync(new MonthlyBudget { Amount = 500 });
        expenseRepo.Setup(r => r.GetByFamilyGroupIdAndDateRangeAsync(command.FamilyGroupId, It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(new List<Expense>());
        var handler = new AddExpenseCommandHandler(expenseRepo.Object, budgetRepo.Object);
        await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
    }
}

public class EditExpenseCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldEditExpense_WhenExpenseExists()
    {
        var expenseRepo = new Mock<IExpenseRepository>();
        var command = new EditExpenseCommand("id1", 200, "Travel", "Card", new DateTime(2024, 6, 2), "Taxi");
        var expense = new Expense { Id = command.Id };
        expenseRepo.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(expense);
        var handler = new EditExpenseCommandHandler(expenseRepo.Object);
        await handler.Handle(command, CancellationToken.None);
        expenseRepo.Verify(r => r.UpdateAsync(It.Is<Expense>(e => e.Id == command.Id && e.Amount == command.Amount)), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenExpenseNotFound()
    {
        var expenseRepo = new Mock<IExpenseRepository>();
        var command = new EditExpenseCommand("id1", 200, "Travel", "Card", new DateTime(2024, 6, 2), "Taxi");
        expenseRepo.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync((Expense)null);
        var handler = new EditExpenseCommandHandler(expenseRepo.Object);
        await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
    }
}

public class DeleteExpenseCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldDeleteExpense()
    {
        var expenseRepo = new Mock<IExpenseRepository>();
        var command = new DeleteExpenseCommand("id1");
        var handler = new DeleteExpenseCommandHandler(expenseRepo.Object);
        await handler.Handle(command, CancellationToken.None);
        expenseRepo.Verify(r => r.DeleteAsync(command.Id), Times.Once);
    }
}
