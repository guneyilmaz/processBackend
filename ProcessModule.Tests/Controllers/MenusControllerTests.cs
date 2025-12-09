using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProcessModule.Application.Features.Menus.Queries;
using ProcessModule.Tests.Common;
using ProcessModule.Tests.Utilities;
using ProcessModule.WebAPI.Controllers;

namespace ProcessModule.Tests.Controllers;

/// <summary>
/// Unit tests for MenusController
/// </summary>
public class MenusControllerTests : ControllerTestBase<MenusController>
{
    private readonly Mock<IMediator> _mockMediator;

    public MenusControllerTests()
    {
        _mockMediator = new Mock<IMediator>();
        Controller = new MenusController(_mockMediator.Object);
    }

    [Fact]
    public async Task GetMenus_WithValidLanguageCode_ReturnsOkResultWithMenus()
    {
        // Arrange
        var languageCode = "tr";
        var expectedMenus = new List<MenuDto>
        {
            TestDataFactory.CreateMenuDto(1, "dashboard", "Anasayfa", "Ana sayfa", "fa fa-home", "/dashboard", null, 1),
            TestDataFactory.CreateMenuDto(2, "users", "Kullanıcılar", "Kullanıcı yönetimi", "fa fa-users", "/users", null, 2)
        };

        _mockMediator
            .Setup(x => x.Send(It.Is<GetMenusQuery>(q => q.LanguageCode == languageCode), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedMenus);

        // Act
        var result = await Controller.GetMenus(languageCode);

        // Assert
        result.Should().NotBeNull();
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedMenus = okResult.Value.Should().BeAssignableTo<List<MenuDto>>().Subject;
        
        returnedMenus.Should().HaveCount(2);
        returnedMenus.Should().BeEquivalentTo(expectedMenus);
        
        _mockMediator.Verify(x => x.Send(It.Is<GetMenusQuery>(q => q.LanguageCode == languageCode), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetMenus_WithEmptyLanguageCode_UsesDefaultTurkish()
    {
        // Arrange
        var expectedMenus = new List<MenuDto>
        {
            TestDataFactory.CreateMenuDto(1, "dashboard", "Anasayfa")
        };

        _mockMediator
            .Setup(x => x.Send(It.Is<GetMenusQuery>(q => q.LanguageCode == "tr"), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedMenus);

        // Act
        var result = await Controller.GetMenus("");

        // Assert
        result.Should().NotBeNull();
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        
        _mockMediator.Verify(x => x.Send(It.Is<GetMenusQuery>(q => q.LanguageCode == "tr"), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetMenus_WhenMediatorThrowsException_ReturnsInternalServerError()
    {
        // Arrange
        var languageCode = "tr";
        var exceptionMessage = "Database connection failed";
        
        _mockMediator
            .Setup(x => x.Send(It.IsAny<GetMenusQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception(exceptionMessage));

        // Act
        var result = await Controller.GetMenus(languageCode);

        // Assert
        result.Should().NotBeNull();
        var objectResult = result.Result.Should().BeOfType<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(500);
        
        var errorResponse = objectResult.Value.Should().BeAssignableTo<object>().Subject;
        errorResponse.Should().NotBeNull();
    }

    [Fact]
    public async Task GetTurkishMenus_CallsGetMenusWithTurkishLanguageCode()
    {
        // Arrange
        var expectedMenus = new List<MenuDto>
        {
            TestDataFactory.CreateMenuDto(1, "dashboard", "Anasayfa")
        };

        _mockMediator
            .Setup(x => x.Send(It.Is<GetMenusQuery>(q => q.LanguageCode == "tr"), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedMenus);

        // Act
        var result = await Controller.GetTurkishMenus();

        // Assert
        result.Should().NotBeNull();
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        
        _mockMediator.Verify(x => x.Send(It.Is<GetMenusQuery>(q => q.LanguageCode == "tr"), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetEnglishMenus_CallsGetMenusWithEnglishLanguageCode()
    {
        // Arrange
        var expectedMenus = new List<MenuDto>
        {
            TestDataFactory.CreateMenuDto(1, "dashboard", "Dashboard")
        };

        _mockMediator
            .Setup(x => x.Send(It.Is<GetMenusQuery>(q => q.LanguageCode == "en"), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedMenus);

        // Act
        var result = await Controller.GetEnglishMenus();

        // Assert
        result.Should().NotBeNull();
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        
        _mockMediator.Verify(x => x.Send(It.Is<GetMenusQuery>(q => q.LanguageCode == "en"), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [InlineData("tr")]
    [InlineData("en")]
    [InlineData("de")]
    [InlineData("fr")]
    public async Task GetMenus_WithDifferentLanguageCodes_PassesCorrectLanguageToMediator(string languageCode)
    {
        // Arrange
        var expectedMenus = new List<MenuDto>();
        
        _mockMediator
            .Setup(x => x.Send(It.Is<GetMenusQuery>(q => q.LanguageCode == languageCode), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedMenus);

        // Act
        await Controller.GetMenus(languageCode);

        // Assert
        _mockMediator.Verify(x => x.Send(It.Is<GetMenusQuery>(q => q.LanguageCode == languageCode), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetMenus_ReturnsEmptyList_WhenNoMenusFound()
    {
        // Arrange
        var languageCode = "tr";
        var emptyMenus = new List<MenuDto>();

        _mockMediator
            .Setup(x => x.Send(It.IsAny<GetMenusQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(emptyMenus);

        // Act
        var result = await Controller.GetMenus(languageCode);

        // Assert
        result.Should().NotBeNull();
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedMenus = okResult.Value.Should().BeAssignableTo<List<MenuDto>>().Subject;
        
        returnedMenus.Should().BeEmpty();
    }
}