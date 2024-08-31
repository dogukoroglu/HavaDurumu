using HavaDurumu.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HavaDurumu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly JwtSettings _jwtSettings;
        public AuthController(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("SignIn")]
        public IActionResult SignIn([FromBody] ApiUser apiUserInfo)
        {
            var apiUser = KimlikDenetimiYap(apiUserInfo);
            if (apiUser == null) return NotFound("Kullanıcı Bulunamadı");

            var token = CreateToken(apiUser);
            return Ok(token);
        }

        private string CreateToken(ApiUser apiUser)
        {
            if (_jwtSettings.Key == null)
            {
                throw new Exception("Jwt Ayarlarındaki Key Değeri Null olamaz!");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,apiUser.UserName!),
                new Claim(ClaimTypes.Role,apiUser.Role!)
            };
            var token = new JwtSecurityToken(_jwtSettings.Issuer,
                _jwtSettings.Audience,
                claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        private ApiUser? KimlikDenetimiYap(ApiUser apiUserInfo)
        {
            return ApiUsers
                .Kullanicilar.FirstOrDefault(x =>
                    x.UserName?.ToLower() == apiUserInfo.UserName
                    && x.Password == apiUserInfo.Password
                );
        }
    }
}
