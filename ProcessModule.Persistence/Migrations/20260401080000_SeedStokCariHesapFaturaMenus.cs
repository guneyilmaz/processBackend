using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProcessModule.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedStokCariHesapFaturaMenus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Get Turkish language ID
            migrationBuilder.Sql(@"
                DECLARE @TurkishLangId INT = (SELECT Id FROM Languages WHERE Code = 'tr');
                DECLARE @EnglishLangId INT = (SELECT Id FROM Languages WHERE Code = 'en');
                
                -- Stok Menüsü
                INSERT INTO Menus ([Key], Icon, Url, ParentId, [Order], IsActive, CreatedAt, IsDeleted)
                VALUES ('stok', 'bi bi-box-seam', '/stok', NULL, 50, 1, GETDATE(), 0);
                
                DECLARE @StokMenuId INT = SCOPE_IDENTITY();
                
                INSERT INTO MenuTranslations (MenuId, LanguageId, Title, CreatedAt, IsDeleted)
                VALUES (@StokMenuId, @TurkishLangId, 'Stok Yönetimi', GETDATE(), 0);
                
                IF @EnglishLangId IS NOT NULL
                BEGIN
                    INSERT INTO MenuTranslations (MenuId, LanguageId, Title, CreatedAt, IsDeleted)
                    VALUES (@StokMenuId, @EnglishLangId, 'Stock Management', GETDATE(), 0);
                END
                
                -- Cari Hesap Menüsü
                INSERT INTO Menus ([Key], Icon, Url, ParentId, [Order], IsActive, CreatedAt, IsDeleted)
                VALUES ('cari-hesap', 'bi bi-people', '/cari-hesap', NULL, 51, 1, GETDATE(), 0);
                
                DECLARE @CariMenuId INT = SCOPE_IDENTITY();
                
                INSERT INTO MenuTranslations (MenuId, LanguageId, Title, CreatedAt, IsDeleted)
                VALUES (@CariMenuId, @TurkishLangId, 'Cari Hesap', GETDATE(), 0);
                
                IF @EnglishLangId IS NOT NULL
                BEGIN
                    INSERT INTO MenuTranslations (MenuId, LanguageId, Title, CreatedAt, IsDeleted)
                    VALUES (@CariMenuId, @EnglishLangId, 'Current Account', GETDATE(), 0);
                END
                
                -- Fatura Menüsü
                INSERT INTO Menus ([Key], Icon, Url, ParentId, [Order], IsActive, CreatedAt, IsDeleted)
                VALUES ('fatura', 'bi bi-receipt', '/fatura', NULL, 52, 1, GETDATE(), 0);
                
                DECLARE @FaturaMenuId INT = SCOPE_IDENTITY();
                
                INSERT INTO MenuTranslations (MenuId, LanguageId, Title, CreatedAt, IsDeleted)
                VALUES (@FaturaMenuId, @TurkishLangId, 'Fatura', GETDATE(), 0);
                
                IF @EnglishLangId IS NOT NULL
                BEGIN
                    INSERT INTO MenuTranslations (MenuId, LanguageId, Title, CreatedAt, IsDeleted)
                    VALUES (@FaturaMenuId, @EnglishLangId, 'Invoice', GETDATE(), 0);
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE FROM MenuTranslations WHERE MenuId IN 
                    (SELECT Id FROM Menus WHERE [Key] IN ('stok', 'cari-hesap', 'fatura'));
                DELETE FROM Menus WHERE [Key] IN ('stok', 'cari-hesap', 'fatura');
            ");
        }
    }
}
