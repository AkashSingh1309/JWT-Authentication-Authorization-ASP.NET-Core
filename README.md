# JWT Auth Demo — ASP.NET Core Web API

A minimal, complete reference implementation of JWT authentication and role-based
authorization in ASP.NET Core 8, with Swagger UI wired up for testing protected
endpoints directly in the browser.

## Features

- User registration & login (`/api/auth/register`, `/api/auth/login`)
- Passwords hashed with `Microsoft.AspNetCore.Identity.PasswordHasher` (never stored in plaintext)
- JWT generation with configurable issuer, audience, key, and expiry
- `[Authorize]` and `[Authorize(Roles = "Admin")]` protected endpoints
- Swagger UI with a working "Authorize" button for pasting bearer tokens
- In-memory user store (swap for EF Core + a real DB in production — see below)

## Project structure

```
JwtAuthDemo/
├── Controllers/
│   ├── AuthController.cs      # register / login endpoints
│   └── DataController.cs      # example protected + public endpoints
├── Models/
│   ├── User.cs
│   └── AuthModels.cs          # LoginRequest, RegisterRequest, AuthResponse
├── Services/
│   ├── TokenService.cs        # builds & signs JWTs
│   └── UserStore.cs           # in-memory user store + password hashing
├── Properties/
│   └── launchSettings.json
├── Program.cs                 # DI, JWT middleware, Swagger config
├── appsettings.json
└── JwtAuthDemo.csproj
```

## Getting started

```bash
git clone <your-repo-url>
cd JwtAuthDemo/JwtAuthDemo
dotnet restore
dotnet run
```

Then open `https://localhost:7214/swagger` (port may vary — check your console output).

### Seeded test account

A default admin user is created in memory on startup:

```
Email:    admin@example.com
Password: Admin@123
```

## Trying it out in Swagger

1. Expand `POST /api/auth/login`, click **Try it out**, and submit:
   ```json
   {
     "email": "admin@example.com",
     "password": "Admin@123"
   }
   ```
2. Copy the `token` value from the response.
3. Scroll to the top of the Swagger page and click the green **Authorize** button.
4. Paste the token as `Bearer <token>` and click **Authorize**, then **Close**.
5. Now call `GET /api/data/secure-data` or `GET /api/data/admin-only` — the token
   is sent automatically on every request from here on.

## Trying it out with curl

```bash
# 1. Log in and grab a token
TOKEN=$(curl -sk -X POST https://localhost:7214/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@example.com","password":"Admin@123"}' | jq -r .token)

# 2. Call a protected endpoint
curl -sk https://localhost:7214/api/data/secure-data \
  -H "Authorization: Bearer $TOKEN"

# 3. Call an admin-only endpoint
curl -sk https://localhost:7214/api/data/admin-only \
  -H "Authorization: Bearer $TOKEN"
```

## Configuration

JWT settings live in `appsettings.json`:

```json
{
  "Jwt": {
    "Key": "THIS_IS_A_DEV_ONLY_SECRET_REPLACE_ME_MIN_32_CHARS!!",
    "Issuer": "JwtAuthDemo",
    "Audience": "JwtAuthDemoClient",
    "ExpiresInMinutes": 60
  }
}
```

**Do not commit real secrets.** For local development, prefer `dotnet user-secrets`:

```bash
dotnet user-secrets init
dotnet user-secrets set "Jwt:Key" "your-real-32-plus-character-secret"
```

For production, use environment variables or a secret manager (Azure Key Vault, AWS
Secrets Manager, etc.) rather than `appsettings.json`.

## How the pieces fit together

1. **Login** (`AuthController.Login`) verifies credentials against the user store
   and calls `TokenService.GenerateToken` to produce a signed JWT containing the
   user's ID, email, and roles as claims.
2. **Every subsequent request** carries that token in the `Authorization: Bearer <token>`
   header.
3. **`UseAuthentication()`** middleware (configured in `Program.cs`) validates the
   token's signature, issuer, audience, and expiry on every incoming request, then
   populates `HttpContext.User` with the decoded claims.
4. **`[Authorize]`** attributes on controller actions check whether the user is
   authenticated (and, if `Roles = "..."` is specified, whether the required role
   claim is present) before the action runs.

No server-side session state is kept — the token itself is the credential, which
is why this scales well but also means individual tokens can't be revoked before
they expire without extra infrastructure (see below).

## Extending this for production

- **Swap `InMemoryUserStore` for EF Core**: replace `IUserStore` with an
  implementation backed by `DbContext`, or migrate to full `AspNetCore.Identity`
  with `UserManager<T>` / `SignInManager<T>`.
- **Add refresh tokens**: issue a long-lived, single-use refresh token alongside
  the short-lived access token so users don't have to re-login every hour.
- **Add token revocation**: store issued JWT IDs (`jti` claim) in a denylist/cache
  (e.g. Redis) checked during validation, for cases like "log out everywhere" or
  compromised accounts.
- **Rate-limit the login endpoint** to slow down brute-force attempts.
- **Use HTTPS everywhere** and never log tokens or passwords.

## License

MIT — use freely for learning or as a starting point for your own projects.
