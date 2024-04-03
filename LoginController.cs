using jwt_token.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace jwt_token.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;
        public LoginController(IConfiguration configuration)
        {
            _config = configuration;
        }

        private Users AuthenticationUser(Users user)
        {
            Users _user = null;
            if(user.UserName=="admin" && user.Password=="12345")
            {
                _user = new Users { UserName = "admin" };
            }
            return _user;
        }

        private string GenerateToken(Users users)
        {
            var claims = new[] { new Claim("id", users.UserName), new Claim("roletype", "Admin") };
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials=new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"], claims, expires: DateTime.Now.AddMinutes(1), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(Users user)
        {
            IActionResult response = Unauthorized();
            var _user = AuthenticationUser(user);
            if(_user != null)
            {
                var token = GenerateToken(_user);
                response = Ok(new { token = token });
            }
            return response;
        }
    }
}
