using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using DailyExpenseManager.Application.Authentication.Handlers;
using DailyExpenseManager.Application.Authentication.Commands;
using DailyExpenseManager.Domain.Entities;
using DailyExpenseManager.Infrastructure.Mongo.Repositories;
using Microsoft.AspNetCore.Identity;
using DailyExpenseManager.Application.Authentication;

public class LoginUserCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnToken_WhenCredentialsAreValid()
    {
        // Arrange
        var userRepositoryMock = new Mock<IUserRepository>();
        var passwordHasherMock = new Mock<IPasswordHasher<User>>();
        var jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();
        var command = new LoginUserCommand { Email = "test@example.com", Password = "password" };
        var user = new User { Email = command.Email, PasswordHash = "hashed" };
        userRepositoryMock.Setup(r => r.GetByEmailAsync(command.Email)).ReturnsAsync(user);
        passwordHasherMock.Setup(h => h.VerifyHashedPassword(user, user.PasswordHash, command.Password)).Returns(PasswordVerificationResult.Success);
        jwtTokenGeneratorMock.Setup(j => j.GenerateToken(user)).Returns("token");
        var handler = new LoginUserCommandHandler(userRepositoryMock.Object, passwordHasherMock.Object, jwtTokenGeneratorMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal("token", result);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenUserNotFound()
    {
        // Arrange
        var userRepositoryMock = new Mock<IUserRepository>();
        var passwordHasherMock = new Mock<IPasswordHasher<User>>();
        var jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();
        var command = new LoginUserCommand { Email = "notfound@example.com", Password = "password" };
        userRepositoryMock.Setup(r => r.GetByEmailAsync(command.Email)).ReturnsAsync((User)null);
        var handler = new LoginUserCommandHandler(userRepositoryMock.Object, passwordHasherMock.Object, jwtTokenGeneratorMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<System.Exception>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenPasswordInvalid()
    {
        // Arrange
        var userRepositoryMock = new Mock<IUserRepository>();
        var passwordHasherMock = new Mock<IPasswordHasher<User>>();
        var jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();
        var command = new LoginUserCommand { Email = "test@example.com", Password = "wrong" };
        var user = new User { Email = command.Email, PasswordHash = "hashed" };
        userRepositoryMock.Setup(r => r.GetByEmailAsync(command.Email)).ReturnsAsync(user);
        passwordHasherMock.Setup(h => h.VerifyHashedPassword(user, user.PasswordHash, command.Password)).Returns(PasswordVerificationResult.Failed);
        var handler = new LoginUserCommandHandler(userRepositoryMock.Object, passwordHasherMock.Object, jwtTokenGeneratorMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<System.Exception>(() => handler.Handle(command, CancellationToken.None));
    }
}
