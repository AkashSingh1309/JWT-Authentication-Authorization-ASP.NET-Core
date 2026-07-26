using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace JWT_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public AuthController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
          
            if (request.Email == "test@example.com" && request.Password == "password123")
            {
                var token = _tokenService.GenerateToken(
                    userId: "1",
                    email: request.Email,
                    roles: new List<string> { "User" }
                );

                return Ok(new { token });
            }

            return Unauthorized();
        }
    }
}
