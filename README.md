# Todo API

JWT Bearer Authentication kullanılan, kullanıcı bazlı yetkilendirmeye sahip basit bir Todo yönetim REST API’si.

Bu proje, authentication ve authorization mantığını doğru kurmayı hedefleyen bir backend denemesidir.

---

## Özellikler

- JWT Bearer Authentication
- Kullanıcı bazlı yetkilendirme (User scoped data)
- CRUD Todo işlemleri
- Register / Login akışı
- BCrypt ile password hashing
- Global exception handling (Middleware)
- Swagger üzerinden JWT ile test edilebilir endpoint’ler
- Katmanlı mimari (Controller / Service / DTO)

---

## Kullanılan Teknolojiler

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- In-Memory Database
- JWT (JSON Web Token)
- BCrypt.Net
- Swagger / OpenAPI

---

## Kurulum & Çalıştırma

```bash
git clone https://github.com/CanerSonerSEKER/ToDo_API.git
cd ToDo_API
dotnet restore
dotnet run

```
## Authentication (JWT)

Bu API, JWT Bearer Authentication kullanır.
Todo endpoint’lerine erişmek için geçerli bir token zorunludur.

## Kullanıcı Kaydı
  POST /api/auth/register
- Kullanıcı oluşturur.
- Register işlemi token üretmez.

## Login & Token Alma

--- POST /api/auth/login
- Başarılı login sonrası JWT token döner.

Örnek response:

{
  "userId": 1,
  "username": "testuser",
  "email": "test@mail.com",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}

## Endpoint’ler
  Auth
  Method	Endpoint	Açıklama
  POST	/api/auth/register	Kullanıcı oluşturur
  POST	/api/auth/login	JWT token üretir
  Todo (Yetkilendirme Gerekli)

- Tüm Todo endpoint’leri [Authorize] ile korunmaktadır.
- Kullanıcılar yalnızca kendi todolarına erişebilir.

## Method	Endpoint	Açıklama
- GET	/api/todos	Kullanıcının tüm todolarını getirir
- GET	/api/todos/{id}	Kullanıcının belirli todo’sunu getirir
- POST	/api/todos	Yeni todo oluşturur
- PUT	/api/todos/{id}	Todo günceller
- DELETE	/api/todos/{id}	Todo siler

## Yetkilendirme Mantığı

- JWT token içinde NameIdentifier claim olarak UserId taşınır
- UserId, HttpContext.User üzerinden alınır
- Todo sorguları UserId ile filtrelenir
- Request üzerinden userId alınmaz

Bu yapı sayesinde:
- Kullanıcılar birbirinin verisini göremez


## Bilinen Kısıtlar

- In-Memory Database kullanıldığı için veriler kalıcı değildir
- Refresh token mekanizması yok
- Role / claim bazlı yetkilendirme yok
- Rate limiting ve account lockout yok
- Unit / Integration testler yok
- Bu proje production ortamı için değil, authentication ve authorization mantığını göstermek amacıyla geliştirilmiştir.
