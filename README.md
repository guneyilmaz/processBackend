# Process Module Backend - .NET 8 Clean Architecture

Bu proje, **Clean Architecture** prensiplerine göre tasarlanmış, **JWT Authentication**, **Entity Framework Core**, **Swagger/OpenAPI**, **Docker** ve **Azure** desteği ile birlikte gelen kapsamlı bir **.NET 8 Web API** uygulamasıdır.

## 🚀 Özellikler

- ✅ **Clean Architecture** (Domain, Application, Infrastructure, WebAPI katmanları)
- ✅ **Entity Framework Core** ile **MSSQL** ve **PostgreSQL** desteği
- ✅ **JWT Authentication** ve **Authorization**
- ✅ **Swagger/OpenAPI** dokümantasyonu
- ✅ **Docker** ve **Docker Compose** desteği
- ✅ **Azure Container Apps** deployment hazır
- ✅ **CORS** yapılandırması (Angular uyumlu)
- ✅ **Repository Pattern** ve **Unit of Work**
- ✅ **Health Check** endpoint'i

## 📁 Proje Yapısı

```
ProcessModule/
├── ProcessModule.Domain/              # Domain katmanı (Entities, Common)
│   ├── Entities/                     # Domain entities
│   └── Common/                       # Base classes
├── ProcessModule.Application/         # Application katmanı (DTOs, Interfaces, Services)
│   ├── DTOs/                        # Data Transfer Objects
│   ├── Interfaces/                  # Repository ve Service interface'leri
│   └── Services/                    # Application services
├── ProcessModule.Infrastructure/      # Infrastructure katmanı (Data, Repositories, Services)
│   ├── Data/                        # DbContext ve Configurations
│   ├── Repositories/                # Repository implementations
│   └── Services/                    # Infrastructure services
├── ProcessModule.WebAPI/             # Presentation katmanı (Controllers, Configuration)
│   ├── Controllers/                 # API Controllers
│   └── Program.cs                   # Application configuration
├── docker-compose.yml               # Docker compose configuration
├── Dockerfile                       # Docker image definition
└── azure-deploy.json               # Azure Resource Manager template
```

## 🛠️ Gereksinimler

- **.NET 8 SDK**
- **SQL Server** veya **PostgreSQL**
- **Docker** (isteğe bağlı)
- **Visual Studio 2022** veya **VS Code**

## ⚡ Hızlı Başlangıç

### 1. Projeyi Klonlayın

```bash
git clone <repository-url>
cd ProcessModuleBackend
```

### 2. Bağımlılıkları Yükleyin

```bash
dotnet restore
```

### 3. Veritabanı Bağlantısını Yapılandırın

`ProcessModule.WebAPI/appsettings.json` dosyasında veritabanı bağlantı dizesini güncelleyin:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ProcessModuleDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### 4. Uygulamayı Çalıştırın

```bash
cd ProcessModule.WebAPI
dotnet run
```

Uygulama **http://localhost:5024** adresinde çalışacaktır.

## 📊 API Endpoints

### Authentication
- `POST /api/auth/login` - Kullanıcı girişi
- `POST /api/auth/register` - Kullanıcı kaydı

### Process Management
- `GET /api/process` - Tüm süreçleri listele
- `GET /api/process/{id}` - Belirli bir süreci getir
- `POST /api/process` - Yeni süreç oluştur
- `PUT /api/process/{id}` - Süreç güncelle
- `DELETE /api/process/{id}` - Süreç sil

### Health Check
- `GET /health` - Sistem sağlık durumu

## 🐳 Docker ile Çalıştırma

### Docker Compose ile Tam Stack

```bash
docker-compose up -d
```

Bu komut aşağıdaki servisleri başlatır:
- **ProcessModule API** (Port 8080)
- **SQL Server** (Port 1433)
- **Nginx Reverse Proxy** (Port 80)

### Sadece API'yi Dockerize Etme

```bash
docker build -t processmodule-api .
docker run -d -p 8080:8080 processmodule-api
```

## ☁️ Azure Deployment

### Azure Container Apps ile Deployment

1. **Azure CLI ile giriş yapın:**
```bash
az login
```

2. **Resource Group oluşturun:**
```bash
az group create --name rg-processmodule --location eastus
```

3. **ARM template ile deploy edin:**
```bash
az deployment group create \
  --resource-group rg-processmodule \
  --template-file azure-deploy.json \
  --parameters sqlAdminPassword="YourStrong@Password" \
              jwtSecretKey="YourJWTSecretKey32Characters!"
```

### GitHub Actions ile Otomatik Deployment

1. Repository secrets'ları ayarlayın:
   - `AZURE_CREDENTIALS`
   - `AZURE_ACR_USERNAME`
   - `AZURE_ACR_PASSWORD`
   - `AZURE_SQL_CONNECTION_STRING`
   - `JWT_SECRET_KEY`

2. `.github/workflows/azure-deploy.yml` dosyası otomatik deployment'ı handle eder.

## 🌐 Angular Integration

Bu API, Angular uygulamaları için optimize edilmiştir:

### CORS Yapılandırması
```csharp
// Angular development ve production URL'leri desteklenir
app.UseCors("AngularApp");
```

### TypeScript Model Örnekleri

```typescript
// Auth models
export interface LoginRequest {
  email: string;
  password: string;
}

export interface AuthResponse {
  token: string;
  email: string;
  firstName: string;
  lastName: string;
  roles: string[];
}

// Process models
export interface Process {
  id: number;
  name: string;
  description?: string;
  status: string;
  ownerId: number;
  ownerName: string;
  dueDate?: Date;
  priority: number;
  createdAt: Date;
  processSteps: ProcessStep[];
}
```

### Angular Service Örneği

```typescript
@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private baseUrl = 'http://localhost:5024/api';

  constructor(private http: HttpClient) {}

  login(credentials: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.baseUrl}/auth/login`, credentials);
  }

  getProcesses(): Observable<Process[]> {
    return this.http.get<Process[]>(`${this.baseUrl}/process`);
  }
}
```

## 🔒 JWT Authentication Kullanımı

### Login İsteği
```bash
curl -X POST "http://localhost:5024/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "user@example.com",
    "password": "password123"
  }'
```

### Korumalı Endpoint'lere Erişim
```bash
curl -X GET "http://localhost:5024/api/process" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

## 🔧 Geliştirme

### Migration Ekleme

```bash
cd ProcessModule.Infrastructure
dotnet ef migrations add MigrationName --startup-project ../ProcessModule.WebAPI
dotnet ef database update --startup-project ../ProcessModule.WebAPI
```

### Test Çalıştırma

```bash
dotnet test
```

## 📝 Konfigürasyon

### appsettings.json Örneği

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ProcessModuleDb;Trusted_Connection=true;MultipleActiveResultSets=true",
    "PostgreSQLConnection": "Host=localhost;Port=5432;Database=ProcessModuleDb;Username=postgres;Password=your_password"
  },
  "JwtSettings": {
    "SecretKey": "YourSecretKeyMustBe32CharactersLong!",
    "Issuer": "ProcessModuleAPI",
    "Audience": "ProcessModuleClient",
    "ExpiryMinutes": "60"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  },
  "AllowedHosts": "*"
}
```

## 🚨 Güvenlik

- JWT token'lar 60 dakika geçerlidir
- Şifreler PBKDF2 ile hash'lenir
- HTTPS zorunludur (production)
- CORS sadece belirtilen origin'lere izin verir
- SQL Injection koruması Entity Framework ile sağlanır

## 🤝 Katkıda Bulunma

1. Fork edin
2. Feature branch oluşturun (`git checkout -b feature/AmazingFeature`)
3. Değişikliklerinizi commit edin (`git commit -m 'Add some AmazingFeature'`)
4. Branch'inizi push edin (`git push origin feature/AmazingFeature`)
5. Pull Request açın

## 📄 Lisans

Bu proje MIT lisansı altında lisanslanmıştır.

## 📞 Destek

Herhangi bir sorun veya öneriniz için GitHub Issues'ı kullanabilirsiniz.

---

**Hazırlayan:** GitHub Copilot ile geliştirilen Clean Architecture .NET 8 projesi