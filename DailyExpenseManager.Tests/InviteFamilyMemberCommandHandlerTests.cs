using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using DailyExpenseManager.Application.FamilyGroups.Handlers;
using DailyExpenseManager.Application.FamilyGroups.Commands;
using DailyExpenseManager.Domain.Entities;
using DailyExpenseManager.Infrastructure.Mongo.Repositories;

public class InviteFamilyMemberCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldAddInvitation_WhenNotAlreadyInvited()
    {
        var repo = new Mock<IFamilyGroupRepository>();
        var group = new FamilyGroup { Id = "g1", PendingInvitations = new() };
        repo.Setup(r => r.GetByIdAsync(group.Id)).ReturnsAsync(group);
        var command = new InviteFamilyMemberCommand(group.Id, "invite@email.com");
        var handler = new InviteFamilyMemberCommandHandler(repo.Object);
        await handler.Handle(command, CancellationToken.None);
        Assert.Contains("invite@email.com", group.PendingInvitations);
        repo.Verify(r => r.UpdateAsync(group), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldNotAddDuplicateInvitation()
    {
        var repo = new Mock<IFamilyGroupRepository>();
        var group = new FamilyGroup { Id = "g1", PendingInvitations = new() { "invite@email.com" } };
        repo.Setup(r => r.GetByIdAsync(group.Id)).ReturnsAsync(group);
        var command = new InviteFamilyMemberCommand(group.Id, "invite@email.com");
        var handler = new InviteFamilyMemberCommandHandler(repo.Object);
        await handler.Handle(command, CancellationToken.None);
        Assert.Single(group.PendingInvitations);
        repo.Verify(r => r.UpdateAsync(group), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldDoNothing_WhenGroupNotFound()
    {
        var repo = new Mock<IFamilyGroupRepository>();
        repo.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((FamilyGroup)null);
        var command = new InviteFamilyMemberCommand("g1", "invite@email.com");
        var handler = new InviteFamilyMemberCommandHandler(repo.Object);
        await handler.Handle(command, CancellationToken.None);
        repo.Verify(r => r.UpdateAsync(It.IsAny<FamilyGroup>()), Times.Never);
    }
}
