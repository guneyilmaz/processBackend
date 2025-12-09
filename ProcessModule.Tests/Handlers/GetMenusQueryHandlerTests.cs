using FluentAssertions;
using Moq;
using ProcessModule.Application.Features.Menus.Queries;
using ProcessModule.Application.Interfaces;
using ProcessModule.Tests.Common;
using ProcessModule.Tests.Utilities;

namespace ProcessModule.Tests.Handlers;

/// <summary>
/// Unit tests for GetMenusQueryHandler
/// </summary>
public class GetMenusQueryHandlerTests : HandlerTestBase<GetMenusQueryHandler>
{
    private readonly Mock<IMenuRepository> _mockMenuRepository;

    public GetMenusQueryHandlerTests()
    {
        _mockMenuRepository = new Mock<IMenuRepository>();
        SetupHandler();
    }

    protected override void SetupHandler()
    {
        Handler = new GetMenusQueryHandler(_mockMenuRepository.Object);
    }

    [Fact]
    public async Task Handle_WithValidLanguageCode_ReturnsMenusWithTranslations()
    {
        // Arrange
        var languageCode = "tr";
        var query = new GetMenusQuery { LanguageCode = languageCode };
        var menuEntities = TestDataFactory.CreateMenusWithTranslations();

        _mockMenuRepository
            .Setup(x => x.GetMenusWithTranslationsAsync(languageCode))
            .ReturnsAsync(menuEntities);

        // Act
        var result = await Handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        
        var firstMenu = result.First();
        firstMenu.Key.Should().Be("dashboard");
        firstMenu.Title.Should().Be("Anasayfa");
        firstMenu.Description.Should().Be("Ana sayfa");
        firstMenu.Icon.Should().Be("fa fa-home");
        firstMenu.Url.Should().Be("/dashboard");
        firstMenu.Order.Should().Be(1);
        firstMenu.IsActive.Should().BeTrue();
        
        var secondMenu = result.Skip(1).First();
        secondMenu.Key.Should().Be("users");
        secondMenu.Title.Should().Be("Kullanıcılar");
        
        _mockMenuRepository.Verify(x => x.GetMenusWithTranslationsAsync(languageCode), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNoTranslationsFound_UsesMenuKeyAsTitle()
    {
        // Arrange
        var languageCode = "fr"; // Non-existing language
        var query = new GetMenusQuery { LanguageCode = languageCode };
        
        var menuWithoutTranslation = TestDataFactory.CreateMenu(1, "dashboard", "fa fa-home", "/dashboard", null, 1);
        menuWithoutTranslation.Translations.Clear(); // Remove translations
        
        var menuEntities = new List<ProcessModule.Domain.Entities.Menu> { menuWithoutTranslation };

        _mockMenuRepository
            .Setup(x => x.GetMenusWithTranslationsAsync(languageCode))
            .ReturnsAsync(menuEntities);

        // Act
        var result = await Handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        
        var menu = result.First();
        menu.Key.Should().Be("dashboard");
        menu.Title.Should().Be("dashboard"); // Falls back to key when no translation found
        menu.Description.Should().BeNull();
    }

    [Fact]
    public async Task Handle_WithHierarchicalMenus_BuildsCorrectHierarchy()
    {
        // Arrange
        var languageCode = "tr";
        var query = new GetMenusQuery { LanguageCode = languageCode };
        
        var language = TestDataFactory.CreateLanguage(1, "tr", "Türkçe");
        
        // Create parent menu
        var parentMenu = TestDataFactory.CreateMenu(1, "admin", "fa fa-cog", "/admin", null, 1);
        parentMenu.Translations.Add(new ProcessModule.Domain.Entities.MenuTranslation
        {
            Id = 1,
            MenuId = 1,
            LanguageId = 1,
            Title = "Yönetim",
            Description = "Yönetim paneli",
            Language = language,
            Menu = parentMenu
        });
        
        // Create child menu
        var childMenu = TestDataFactory.CreateMenu(2, "users", "fa fa-users", "/admin/users", 1, 1);
        childMenu.Translations.Add(new ProcessModule.Domain.Entities.MenuTranslation
        {
            Id = 2,
            MenuId = 2,
            LanguageId = 1,
            Title = "Kullanıcılar",
            Description = "Kullanıcı yönetimi",
            Language = language,
            Menu = childMenu
        });

        var menuEntities = new List<ProcessModule.Domain.Entities.Menu> { parentMenu, childMenu };

        _mockMenuRepository
            .Setup(x => x.GetMenusWithTranslationsAsync(languageCode))
            .ReturnsAsync(menuEntities);

        // Act
        var result = await Handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1); // Only root menus should be returned
        
        var rootMenu = result.First();
        rootMenu.Key.Should().Be("admin");
        rootMenu.Title.Should().Be("Yönetim");
        rootMenu.Children.Should().HaveCount(1);
        
        var childMenuDto = rootMenu.Children.First();
        childMenuDto.Key.Should().Be("users");
        childMenuDto.Title.Should().Be("Kullanıcılar");
        childMenuDto.ParentId.Should().Be(1);
    }

    [Fact]
    public async Task Handle_WithEmptyMenuList_ReturnsEmptyList()
    {
        // Arrange
        var languageCode = "tr";
        var query = new GetMenusQuery { LanguageCode = languageCode };
        var emptyMenus = new List<ProcessModule.Domain.Entities.Menu>();

        _mockMenuRepository
            .Setup(x => x.GetMenusWithTranslationsAsync(languageCode))
            .ReturnsAsync(emptyMenus);

        // Act
        var result = await Handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
        
        _mockMenuRepository.Verify(x => x.GetMenusWithTranslationsAsync(languageCode), Times.Once);
    }

    [Fact]
    public async Task Handle_WithRepositoryException_ThrowsException()
    {
        // Arrange
        var languageCode = "tr";
        var query = new GetMenusQuery { LanguageCode = languageCode };
        var exceptionMessage = "Database connection failed";

        _mockMenuRepository
            .Setup(x => x.GetMenusWithTranslationsAsync(languageCode))
            .ThrowsAsync(new Exception(exceptionMessage));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => Handler.Handle(query, CancellationToken.None));
        exception.Message.Should().Be(exceptionMessage);
    }

    [Theory]
    [InlineData("tr")]
    [InlineData("en")]
    [InlineData("de")]
    [InlineData("fr")]
    public async Task Handle_WithDifferentLanguageCodes_CallsRepositoryWithCorrectLanguage(string languageCode)
    {
        // Arrange
        var query = new GetMenusQuery { LanguageCode = languageCode };
        var emptyMenus = new List<ProcessModule.Domain.Entities.Menu>();

        _mockMenuRepository
            .Setup(x => x.GetMenusWithTranslationsAsync(languageCode))
            .ReturnsAsync(emptyMenus);

        // Act
        await Handler.Handle(query, CancellationToken.None);

        // Assert
        _mockMenuRepository.Verify(x => x.GetMenusWithTranslationsAsync(languageCode), Times.Once);
    }
}