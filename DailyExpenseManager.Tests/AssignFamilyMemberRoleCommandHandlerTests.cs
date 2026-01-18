using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using DailyExpenseManager.Application.FamilyGroups.Handlers;
using DailyExpenseManager.Application.FamilyGroups.Commands;
using DailyExpenseManager.Domain.Entities;
using DailyExpenseManager.Infrastructure.Mongo.Repositories;
using System.Collections.Generic;

public class AssignFamilyMemberRoleCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldAssignRole_WhenMemberExists()
    {
        var repo = new Mock<IFamilyGroupRepository>();
        var group = new FamilyGroup { Id = "g1", Members = new List<FamilyMember> { new FamilyMember { UserId = "u1", Role = UserRole.User } } };
        repo.Setup(r => r.GetByIdAsync(group.Id)).ReturnsAsync(group);
        var command = new AssignFamilyMemberRoleCommand(group.Id, "u1", UserRole.Admin);
        var handler = new AssignFamilyMemberRoleCommandHandler(repo.Object);
        await handler.Handle(command, CancellationToken.None);
        Assert.Equal(UserRole.Admin, group.Members[0].Role);
        repo.Verify(r => r.UpdateAsync(group), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldDoNothing_WhenGroupNotFound()
    {
        var repo = new Mock<IFamilyGroupRepository>();
        repo.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((FamilyGroup)null);
        var command = new AssignFamilyMemberRoleCommand("g1", "u1", UserRole.Admin);
        var handler = new AssignFamilyMemberRoleCommandHandler(repo.Object);
        await handler.Handle(command, CancellationToken.None);
        repo.Verify(r => r.UpdateAsync(It.IsAny<FamilyGroup>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldDoNothing_WhenMemberNotFound()
    {
        var repo = new Mock<IFamilyGroupRepository>();
        var group = new FamilyGroup { Id = "g1", Members = new List<FamilyMember>() };
        repo.Setup(r => r.GetByIdAsync(group.Id)).ReturnsAsync(group);
        var command = new AssignFamilyMemberRoleCommand(group.Id, "u1", UserRole.Admin);
        var handler = new AssignFamilyMemberRoleCommandHandler(repo.Object);
        await handler.Handle(command, CancellationToken.None);
        repo.Verify(r => r.UpdateAsync(group), Times.Never);
    }
}
