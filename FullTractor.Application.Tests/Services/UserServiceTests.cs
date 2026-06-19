using FullTractor.Application.DTOs;
using FullTractor.Application.Enums;
using FullTractor.Application.Services;
using FullTractor.Domain.Entities;
using FullTractor.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace FullTractor.Application.Tests.Services;

public class UserServiceTests
{
    [Fact]
    public async Task GetUserByEmailAsync_UserNotExist_ReturnNotFound()
    {
        //Arrange
        var mockUserRepo = new Mock<IUserRepository>();
        var mockPasswordHasher = new Mock<IPasswordHasher<UserRequest>>();

        mockUserRepo.Setup(r => r.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync((User)null!);

        var service = new UserService(mockUserRepo.Object, mockPasswordHasher.Object);

        //Act
        var result = await service.GetUserByEmailAsync("");

        //Assert
        Assert.Equal(Status.NotFound, result.Status);
    }

    [Fact]
    public async Task CreateUserAsync_EmailExist_ReturnEmailExist()
    {
        //Arrange
        var mockUserRepo = new Mock<IUserRepository>();
        var mockPasswordHasher = new Mock<IPasswordHasher<UserRequest>>();

        mockUserRepo.Setup(r => r.GetUserByEmailAsync("correoIgual@gmail.com")).ReturnsAsync(new User { Email = "correoIgual@gmail.com", Name = "", Address = "", CellPhone = "", City = "", LastName = "" });

        var service = new UserService(mockUserRepo.Object, mockPasswordHasher.Object);

        //Act
        var result = await service.CreateUserAsync(new UserRequest { Email = "correoIgual@gmail.com", PasswordHash = "123" });

        //Assert
        Assert.Equal(Status.EmailExists, result.Status);
    }

    [Fact]
    public async Task CreateUserAsync_EmailNotExist_ReturnSuccess()
    {
        //Arrange
        var mockUserRepo = new Mock<IUserRepository>();
        var mockPasswordHasher = new Mock<IPasswordHasher<UserRequest>>();

        mockUserRepo.Setup(r => r.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync((User)null!);
        mockPasswordHasher.Setup(r => r.HashPassword(It.IsAny<UserRequest>(), It.IsAny<string>())).Returns("passwordHash");
        mockUserRepo.Setup(r => r.CreateUserAsync(It.IsAny<User>())).ReturnsAsync(new User { Name = "", LastName = "", Address = "", CellPhone = "", City = "", Email = "" });

        var service = new UserService(mockUserRepo.Object, mockPasswordHasher.Object);

        //Act
        var result = await service.CreateUserAsync(new UserRequest { Email = "correoIgual@gmail.com", PasswordHash = "123" });

        //Assert
        Assert.Equal(Status.Success, result.Status);
    }
}