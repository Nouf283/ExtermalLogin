using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System;
using ExternalLoginWeb.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ExternalLoginWeb.Controllers
{
    public class AuthController : Controller
    {
        private readonly ExternalLoginDbContext _externalLoginDbContext;
        private readonly IConfiguration _configuration;

        public AuthController(ExternalLoginDbContext externalLoginDbContext, IConfiguration configuration)
        {
            _externalLoginDbContext = externalLoginDbContext;
            this._configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] User credential)
        {
            var user = await _externalLoginDbContext.Users.Where(x => x.UserName == credential.UserName && x.Password == credential.Password).FirstOrDefaultAsync();
            if (user != null)
            {
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name,user.UserName),
                        new Claim(ClaimTypes.Email,user.Email),
                    };
                var expiresAt = DateTime.UtcNow.AddMinutes(10);
                return Ok(new
                {
                    access_token = CreateToken(claims, expiresAt),
                    expires_at = expiresAt
                });
            }

            ModelState.AddModelError("UnAuthorized", "You are not allowed to access the endpoint");
            return  Unauthorized(ModelState);
        }

        private string CreateToken(IEnumerable<Claim> claims, DateTime expireAt)
        {
            var secretKey = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("secretKey"));
            var jwt = new JwtSecurityToken(
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expireAt,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(secretKey),
                SecurityAlgorithms.HmacSha256Signature));
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

    }
}

