JWT Auth Demo – ASP.NET Core 8 Web API

A simple ASP.NET Core 8 Web API project demonstrating JWT Authentication and Role-Based Authorization. This project includes secure user registration, login, JWT token generation, and protected API endpoints with Swagger support for easy testing.

🚀 Features
🔐 User Registration & Login
🔒 Password hashing using PasswordHasher
🎫 JWT Token Authentication
👤 Role-Based Authorization (User & Admin)
📖 Swagger UI with Authorize support
💾 In-memory user store (easy to replace with EF Core)
🛠️ Tech Stack
ASP.NET Core 8 Web API
JWT Authentication
C#
Swagger (OpenAPI)
Microsoft Identity PasswordHasher
📂 Project Structure
Controllers/
Models/
Services/
Program.cs
appsettings.json
▶️ Getting Started
git clone <repository-url>
cd JwtAuthDemo
dotnet restore
dotnet run

Open Swagger:

https://localhost:<port>/swagger
👤 Demo Account
Email: admin@example.com
Password: Admin@123
🔑 JWT Configuration

Configure JWT settings in appsettings.json:

"Jwt": {
  "Key": "YourSecretKey",
  "Issuer": "JwtAuthDemo",
  "Audience": "JwtAuthDemoClient",
  "ExpiresInMinutes": 60
}

For production, store secrets using User Secrets, Environment Variables, or Azure Key Vault instead of appsettings.json.

📌 API Endpoints
POST /api/auth/register – Register a new user
POST /api/auth/login – Generate JWT token
GET /api/data/secure-data – Authenticated users only
GET /api/data/admin-only – Admin users only
📈 Future Improvements
EF Core & SQL Server integration
Refresh Tokens
Token Revocation
Login Rate Limiting
