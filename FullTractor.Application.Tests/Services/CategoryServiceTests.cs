using FullTractor.Application.Enums;
using FullTractor.Application.Services;
using FullTractor.Domain.Entities;
using FullTractor.Domain.Interfaces;
using Moq;

namespace FullTractor.Application.Tests.Services;

public class CategoryServiceTests
{
    [Fact]
    public async Task GetCategoryByIdAsync_CategoryExist_ReturnSuccess()
    {
        //Arrange
        var mockCategoryRepo = new Mock<ICategoryRepository>();
        var mockProductRepo = new Mock<IProductRepository>();

        mockCategoryRepo.Setup(r => r.GetCategoryByIdAsync(5)).ReturnsAsync(new Category { Id = 5, Name = "Lubricantes" });

        var service = new CategoryService(mockCategoryRepo.Object, mockProductRepo.Object);

        //Act
        var result = await service.GetCategoryByIdAsync(5);

        //Assert
        Assert.Equal(Status.Success, result.Status);
        Assert.NotNull(result.Data);
        Assert.Equal("Lubricantes", result.Data.Name);
    }

    [Fact]
    public async Task GetCategoryByIdAsync_CategoryNotExist_ReturnNotFound()
    {
        //Arrange
        var mockCategoryRepo = new Mock<ICategoryRepository>();
        var mockProductRepo = new Mock<IProductRepository>();

        mockCategoryRepo.Setup(r => r.GetCategoryByIdAsync(It.IsAny<int>())).ReturnsAsync((Category)null!);

        var service = new CategoryService(mockCategoryRepo.Object, mockProductRepo.Object);

        //Act
        var result = await service.GetCategoryByIdAsync(5);

        //Assert
        Assert.Equal(Status.NotFound, result.Status);
        Assert.Null(result.Data);
    }

    [Fact]
    public async Task DeleteCategoryAsync_CategoryNotFound_ReturnNotFound()
    {
        //Arrange
        var mockCategoryRepo = new Mock<ICategoryRepository>();
        var mockProductRepo = new Mock<IProductRepository>();

        mockCategoryRepo.Setup(r => r.GetCategoryByIdAsync(It.IsAny<int>())).ReturnsAsync((Category)null!);

        var service = new CategoryService(mockCategoryRepo.Object, mockProductRepo.Object);

        //Act
        var result = await service.GetCategoryByIdAsync(5);

        //Assert
        Assert.Equal(Status.NotFound, result.Status);
        Assert.Null(result.Data);
    }

    [Fact]
    public async Task DeleteCategoryAsync_CategoryHasProduct_ReturnCategoryHasProductsRelated()
    {
        //Arrange
        var mockCategoryRepo = new Mock<ICategoryRepository>();
        var mockProductRepo = new Mock<IProductRepository>();

        mockCategoryRepo.Setup(r => r.GetCategoryByIdAsync(It.IsAny<int>())).ReturnsAsync(new Category { Id = 5, Name = "Lubricantes" });
        mockProductRepo.Setup(r => r.GetProductsByCategoryIdAsync(It.IsAny<int>())).ReturnsAsync(new List<Product> { new Product { Name = "Aceite", Description = "Aceite de motor" } });

        var service = new CategoryService(mockCategoryRepo.Object, mockProductRepo.Object);

        //Act
        var result = await service.DeleteCategoryAsync(5);

        //Assert
        Assert.Equal(Status.CategoryHasProductsRelated, result.Status);
        Assert.Null(result.Data);
    }

    [Fact]
    public async Task DeleteCategoryAsync_CategoryDeleteError_ReturnDeleteError()
    {
        //Arrange
        var mockCategoryRepo = new Mock<ICategoryRepository>();
        var mockProductRepo = new Mock<IProductRepository>();

        mockCategoryRepo.Setup(r => r.GetCategoryByIdAsync(It.IsAny<int>())).ReturnsAsync(new Category { Id = 5, Name = "Lubricantes" });
        mockProductRepo.Setup(r => r.GetProductsByCategoryIdAsync(It.IsAny<int>())).ReturnsAsync([]);
        mockCategoryRepo.Setup(r => r.DeleteCategoryAsync(It.IsAny<int>())).ReturnsAsync(false);

        var service = new CategoryService(mockCategoryRepo.Object, mockProductRepo.Object);

        //Act
        var result = await service.DeleteCategoryAsync(5);

        //Assert
        Assert.Equal(Status.DeleteError, result.Status);
        Assert.Null(result.Data);
    }

    [Fact]
    public async Task DeleteCategoryAsync_CategoryDeleteSuccess_ReturnOk()
    {
        //Arrange
        var mockCategoryRepo = new Mock<ICategoryRepository>();
        var mockProductRepo = new Mock<IProductRepository>();

        mockCategoryRepo.Setup(r => r.GetCategoryByIdAsync(It.IsAny<int>())).ReturnsAsync(new Category { Id = 5, Name = "Lubricantes" });
        mockProductRepo.Setup(r => r.GetProductsByCategoryIdAsync(It.IsAny<int>())).ReturnsAsync([]);
        mockCategoryRepo.Setup(r => r.DeleteCategoryAsync(It.IsAny<int>())).ReturnsAsync(true);

        var service = new CategoryService(mockCategoryRepo.Object, mockProductRepo.Object);

        //Act
        var result = await service.DeleteCategoryAsync(5);

        //Assert
        Assert.Equal(Status.Success, result.Status);
        Assert.Null(result.Data);
    }
}
