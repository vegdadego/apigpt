using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IConfiguration _config;
        public UsuariosController(IConfiguration config)
        {
            _config = config;
        }

        public class LoginDto
        {
            public string Nombre { get; set; } = string.Empty;
            public string Contraseña { get; set; } = string.Empty;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            if (dto.Nombre != "admin" || dto.Contraseña != "1234")
                return Unauthorized(new { message = "Credenciales incorrectas" });

            var jwtSettings = _config.GetSection("Jwt");
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, dto.Nombre)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new { token = tokenString });
        }
    }
} 