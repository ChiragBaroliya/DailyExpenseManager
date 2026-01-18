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

public class RegisterUserCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldRegisterUserAndReturnToken()
    {
        // Arrange
        var userRepositoryMock = new Mock<IUserRepository>();
        var passwordHasherMock = new Mock<IPasswordHasher<User>>();
        var jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();
        var command = new RegisterUserCommand
        {
            Email = "test@example.com",
            Password = "password",
            FirstName = "Test",
            LastName = "User",
            DateOfBirth = new System.DateTime(2000, 1, 1)
        };
        userRepositoryMock.Setup(r => r.GetByEmailAsync(command.Email)).ReturnsAsync((User)null);
        passwordHasherMock.Setup(h => h.HashPassword(It.IsAny<User>(), command.Password)).Returns("hashed");
        jwtTokenGeneratorMock.Setup(j => j.GenerateToken(It.IsAny<User>())).Returns("token");
        var handler = new RegisterUserCommandHandler(userRepositoryMock.Object, passwordHasherMock.Object, jwtTokenGeneratorMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal("token", result);
        userRepositoryMock.Verify(r => r.AddAsync(It.Is<User>(u => u.Email == command.Email)), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenEmailExists()
    {
        // Arrange
        var userRepositoryMock = new Mock<IUserRepository>();
        var passwordHasherMock = new Mock<IPasswordHasher<User>>();
        var jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();
        var command = new RegisterUserCommand
        {
            Email = "test@example.com",
            Password = "password"
        };
        userRepositoryMock.Setup(r => r.GetByEmailAsync(command.Email)).ReturnsAsync(new User());
        var handler = new RegisterUserCommandHandler(userRepositoryMock.Object, passwordHasherMock.Object, jwtTokenGeneratorMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<System.Exception>(() => handler.Handle(command, CancellationToken.None));
    }
}
