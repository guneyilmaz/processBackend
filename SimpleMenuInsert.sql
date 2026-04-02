-- Basit INSERT - Direk çalıştırılabilir
-- Türkçe dil ID'si 1 varsayılıyor

-- Stok Menüsü
INSERT INTO Menus ([Key], Icon, Url, ParentId, [Order], IsActive, CreatedAt, IsDeleted)
VALUES ('stok', 'bi bi-box-seam', '/stok', NULL, 50, 1, GETDATE(), 0);

DECLARE @StokMenuId INT = @@IDENTITY;

INSERT INTO MenuTranslations (MenuId, LanguageId, Title, CreatedAt, IsDeleted)
VALUES (@StokMenuId, 1, 'Stok Yönetimi', GETDATE(), 0);

-- Cari Hesap Menüsü
INSERT INTO Menus ([Key], Icon, Url, ParentId, [Order], IsActive, CreatedAt, IsDeleted)
VALUES ('cari-hesap', 'bi bi-people', '/cari-hesap', NULL, 51, 1, GETDATE(), 0);

DECLARE @CariMenuId INT = @@IDENTITY;

INSERT INTO MenuTranslations (MenuId, LanguageId, Title, CreatedAt, IsDeleted)
VALUES (@CariMenuId, 1, 'Cari Hesap', GETDATE(), 0);

-- Fatura Menüsü
INSERT INTO Menus ([Key], Icon, Url, ParentId, [Order], IsActive, CreatedAt, IsDeleted)
VALUES ('fatura', 'bi bi-receipt', '/fatura', NULL, 52, 1, GETDATE(), 0);

DECLARE @FaturaMenuId INT = @@IDENTITY;

INSERT INTO MenuTranslations (MenuId, LanguageId, Title, CreatedAt, IsDeleted)
VALUES (@FaturaMenuId, 1, 'Fatura', GETDATE(), 0);

-- Kontrol
SELECT m.Id, m.[Key], m.Url, mt.Title 
FROM Menus m
LEFT JOIN MenuTranslations mt ON m.Id = mt.MenuId
WHERE m.[Key] IN ('stok', 'cari-hesap', 'fatura');
