# ProcessModule.Tests

Bu proje, ProcessModule uygulaması için kapsamlı unit testleri içerir.

## Test Yapısı

### 📁 Klasör Organizasyonu
```
ProcessModule.Tests/
├── Common/                 # Base test sınıfları
│   ├── ControllerTestBase.cs
│   └── HandlerTestBase.cs
├── Controllers/            # Controller testleri
│   └── MenusControllerTests.cs
├── Handlers/              # Handler testleri
│   └── GetMenusQueryHandlerTests.cs
└── Utilities/             # Test yardımcıları
    └── TestDataFactory.cs
```

### 🧪 Test Kategorileri

#### Controller Tests
- **ControllerTestBase**: Tüm controller testleri için base class
- **MenusControllerTests**: MenusController için kapsamlı testler
  - Başarılı senaryolar
  - Hata durumları  
  - Farklı dil kodları
  - Validation testleri

#### Handler Tests
- **HandlerTestBase**: Tüm handler testleri için base class
- **GetMenusQueryHandlerTests**: GetMenusQueryHandler için testler
  - Repository entegrasyonu
  - Hiyerarşik menü yapısı
  - Translation mapping
  - Exception handling

### 🛠️ Kullanılan Test Kütüphaneleri

- **xUnit**: Test framework
- **Moq**: Mocking framework
- **FluentAssertions**: Assertion library
- **Microsoft.AspNetCore.Mvc.Testing**: Integration testing

### 🚀 Test Çalıştırma

```bash
# Tüm testleri çalıştır
dotnet test

# Specific test class çalıştır
dotnet test --filter "MenusControllerTests"

# Coverage ile çalıştır
dotnet test --collect:"XPlat Code Coverage"
```

### 📋 Test Yazım Standartları

#### Naming Convention
```csharp
[MethodName]_[Scenario]_[ExpectedResult]
```

#### Test Structure (AAA Pattern)
```csharp
[Fact]
public async Task GetMenus_WithValidLanguageCode_ReturnsOkResultWithMenus()
{
    // Arrange - Test verilerini hazırla
    var languageCode = "tr";
    var expectedMenus = TestDataFactory.CreateMenus();
    
    // Act - Test edilecek metodu çalıştır
    var result = await Controller.GetMenus(languageCode);
    
    // Assert - Sonuçları doğrula
    result.Should().NotBeNull();
    // ...
}
```

### 🎯 Future Endpoint Test Structure

Gelecekte eklenecek endpoint'ler için test yapısı:

1. **Controller Tests**: Her controller için ayrı test class'ı
2. **Handler Tests**: Her handler için ayrı test class'ı  
3. **Repository Tests**: Integration testler için
4. **End-to-End Tests**: Full application testleri

#### Örnek Yeni Controller Test
```csharp
public class UsersControllerTests : ControllerTestBase<UsersController>
{
    // Test methods...
}
```

### 📊 Test Coverage Hedefleri

- **Controller Tests**: %90+ coverage
- **Handler Tests**: %95+ coverage
- **Business Logic**: %100% coverage
- **Integration Tests**: Core scenarios

### 🔧 Mock Strategy

- **Repository Layer**: Her zaman mock
- **External Services**: Mock kullan
- **Domain Logic**: Gerçek objeler kullan
- **Database**: In-memory veya mock

Bu test yapısı, gelecekte eklenecek tüm endpoint'ler için tutarlı ve maintainable bir test stratejisi sağlar.