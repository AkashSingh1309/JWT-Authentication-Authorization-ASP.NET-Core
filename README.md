# JWT Authentication – ASP.NET Core 8 Web API

A simple ASP.NET Core 8 Web API project demonstrating JWT Authentication and Role-Based Authorization.

## Features

- User Registration & Login
- JWT Token Authentication
- Password Hashing
- Role-Based Authorization
- Swagger UI for API Testing
- Protected API Endpoints

## Tech Stack

- ASP.NET Core 8
- C#
- JWT Authentication
- Swagger (OpenAPI)

## Project Structure

```
JwtAuthDemo/
├── Controllers/
├── Models/
├── Services/
├── Program.cs
├── appsettings.json
└── JwtAuthDemo.csproj
```

## Getting Started

```bash
git clone <repository-url>
cd JwtAuthDemo
dotnet restore
dotnet run
```

Open Swagger:

```
https://localhost:<port>/swagger
```

## Demo Account

- **Email:** admin@example.com
- **Password:** Admin@123

## API Endpoints

- `POST /api/auth/register` – Register a user
- `POST /api/auth/login` – Login and get JWT token
- `GET /api/data/secure-data` – Protected endpoint
- `GET /api/data/admin-only` – Admin-only endpoint

## License

Free to use and modify for learning purposes.
