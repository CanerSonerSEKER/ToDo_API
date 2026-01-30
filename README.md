# Todo API

## Açıklama
Basit bir Todo yönetim REST API'si

## Teknolojiler
- .NET 8
- ASP.NET Core Web API
- In-Memory Storage

## Çalıştırma
```bash
git clone https://github.com/CanerSonerSEKER/ToDo_API.git
cd ToDo_API/TodoAPI
dotnet restore
dotnet run
```

## Endpoint'ler
| Method | Endpoint | Açıklama |
|--------|----------|----------|
| GET | /api/todos | Tüm todoları listele |
| GET | /api/todos/{id} | Belirli bir todo'yu getir |
| POST | /api/todos | Yeni todo oluştur |
| PUT | /api/todos/{id} | Todo güncelle |
| DELETE | /api/todos/{id} | Todo sil |

## Mimari
- **Controller:** HTTP isteklerini karşılar
- **Service:** Business logic
- **Storage:** In-memory data store
- **DTO:** Data transfer objects
