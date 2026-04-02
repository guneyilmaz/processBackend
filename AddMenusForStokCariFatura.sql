-- Stok, Cari Hesap ve Fatura Menülerini Ekle
-- Bu script'i SQL Server Management Studio veya başka bir SQL tool'da çalıştırın

-- Önce mevcut maksimum ID'leri bulalım
DECLARE @MaxMenuId INT = (SELECT ISNULL(MAX(Id), 0) FROM Menus);
DECLARE @MaxTranslationId INT = (SELECT ISNULL(MAX(Id), 0) FROM MenuTranslations);

-- Stok Menüsü
INSERT INTO Menus (Id, [Key], Icon, Url, ParentId, [Order], IsActive, CreatedAt, IsDeleted)
VALUES 
(@MaxMenuId + 1, 'stok', 'bi bi-box-seam', '/stok', NULL, 50, 1, GETDATE(), 0);

-- Cari Hesap Menüsü
INSERT INTO Menus (Id, [Key], Icon, Url, ParentId, [Order], IsActive, CreatedAt, IsDeleted)
VALUES 
(@MaxMenuId + 2, 'cari-hesap', 'bi bi-people', '/cari-hesap', NULL, 51, 1, GETDATE(), 0);

-- Fatura Menüsü
INSERT INTO Menus (Id, [Key], Icon, Url, ParentId, [Order], IsActive, CreatedAt, IsDeleted)
VALUES 
(@MaxMenuId + 3, 'fatura', 'bi bi-receipt', '/fatura', NULL, 52, 1, GETDATE(), 0);

-- Dil ID'sini bulalım (Türkçe)
DECLARE @TurkishLangId INT = (SELECT Id FROM Languages WHERE Code = 'tr');
DECLARE @EnglishLangId INT = (SELECT Id FROM Languages WHERE Code = 'en');

-- Stok Menüsü Çevirileri
INSERT INTO MenuTranslations (Id, MenuId, LanguageId, Title, CreatedAt, IsDeleted)
VALUES 
(@MaxTranslationId + 1, @MaxMenuId + 1, @TurkishLangId, 'Stok Yönetimi', GETDATE(), 0);

IF @EnglishLangId IS NOT NULL
BEGIN
    INSERT INTO MenuTranslations (Id, MenuId, LanguageId, Title, CreatedAt, IsDeleted)
    VALUES 
    (@MaxTranslationId + 2, @MaxMenuId + 1, @EnglishLangId, 'Stock Management', GETDATE(), 0);
END

-- Cari Hesap Menüsü Çevirileri
INSERT INTO MenuTranslations (Id, MenuId, LanguageId, Title, CreatedAt, IsDeleted)
VALUES 
(@MaxTranslationId + 3, @MaxMenuId + 2, @TurkishLangId, 'Cari Hesap', GETDATE(), 0);

IF @EnglishLangId IS NOT NULL
BEGIN
    INSERT INTO MenuTranslations (Id, MenuId, LanguageId, Title, CreatedAt, IsDeleted)
    VALUES 
    (@MaxTranslationId + 4, @MaxMenuId + 2, @EnglishLangId, 'Current Account', GETDATE(), 0);
END

-- Fatura Menüsü Çevirileri
INSERT INTO MenuTranslations (Id, MenuId, LanguageId, Title, CreatedAt, IsDeleted)
VALUES 
(@MaxTranslationId + 5, @MaxMenuId + 3, @TurkishLangId, 'Fatura', GETDATE(), 0);

IF @EnglishLangId IS NOT NULL
BEGIN
    INSERT INTO MenuTranslations (Id, MenuId, LanguageId, Title, CreatedAt, IsDeleted)
    VALUES 
    (@MaxTranslationId + 6, @MaxMenuId + 3, @EnglishLangId, 'Invoice', GETDATE(), 0);
END

GO

-- Kontrol Et
SELECT 
    m.Id,
    m.[Key],
    m.Icon,
    m.Url,
    m.[Order],
    m.IsActive,
    mt.LanguageId,
    mt.Title,
    l.Code as LanguageCode
FROM Menus m
LEFT JOIN MenuTranslations mt ON m.Id = mt.MenuId
LEFT JOIN Languages l ON mt.LanguageId = l.Id
WHERE m.[Key] IN ('stok', 'cari-hesap', 'fatura')
ORDER BY m.[Order], l.Code;
