using CheckPilot.Models;
using CheckPilot.Server.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CheckPilot.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginRepository _loginRepository;
        private readonly IConfiguration _configuration;

        public LoginController(ILoginRepository loginRepository, IConfiguration configuration)
        {
            _loginRepository = loginRepository;
            _configuration = configuration;

        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _loginRepository.AuthenticateUserAsync(request.Identifier, request.Password);

            if (user == null)
                return Unauthorized("Usuario, correo o teléfono incorrecto, o la contraseña es inválida.");

            UserContext.UserNameContext = user.UserName ?? "Usuario";

            var token = GenerateJwtToken(user);

            return Ok(new
            {
                token,
                user = new LoginResponse
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    UserRoleId = user.UserRoleId,
                    UserRole = user.UserRole
                }
            });
        }

            private string GenerateJwtToken(User user)
            {
                var claims = new[]
                {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("UserId", user.UserId.ToString()),
            new Claim("UserRole", user.UserRoleId.ToString()),
        };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }

    }

    public class LoginRequest
    {
        public string? Identifier { get; set; }
        public string? Password { get; set; }
    }

    public class LoginResponse
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public int? UserRoleId { get; set; }
        public string? UserRole { get; set; }
    }
}
