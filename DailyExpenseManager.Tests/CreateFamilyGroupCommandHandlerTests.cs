using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using DailyExpenseManager.Application.FamilyGroups.Handlers;
using DailyExpenseManager.Application.FamilyGroups.Commands;
using DailyExpenseManager.Domain.Entities;
using DailyExpenseManager.Infrastructure.Mongo.Repositories;
using System.Collections.Generic;

public class CreateFamilyGroupCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateFamilyGroup()
    {
        var repo = new Mock<IFamilyGroupRepository>();
        var command = new CreateFamilyGroupCommand("Smiths", "admin1", "admin@email.com");
        var handler = new CreateFamilyGroupCommandHandler(repo.Object);
        var result = await handler.Handle(command, CancellationToken.None);
        repo.Verify(r => r.AddAsync(It.Is<FamilyGroup>(g => g.Name == command.Name && g.Members.Exists(m => m.UserId == command.AdminUserId && m.Role == UserRole.Admin))), Times.Once);
    }
}
