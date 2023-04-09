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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace ExternalLoginWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        [Route("adduser")]
        [AllowAnonymous]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            // employee.Id = (int)Guid.NewGuid();
            await _externalLoginDbContext.Users.AddAsync(user);
            await _externalLoginDbContext.SaveChangesAsync();
            return Ok(user);
        }

        [HttpPost]
        [Route("authenticateuser")]
        [AllowAnonymous]
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

        [AllowAnonymous]
        [HttpPost(nameof(ExternalLogin))]
        public IActionResult ExternalLogin(User model)
        {
            //if (model == null || !ModelState.IsValid)
            //{
            //    return null;
            //}

            //var properties = new AuthenticationProperties { RedirectUri = _authenticationAppSettings.External.RedirectUri };

            //return Challenge(properties, model.Provider);
            return null;
        }

        [AllowAnonymous]
        [HttpGet(nameof(ExternalLoginCallback))]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            //Here we can retrieve the claims
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return null;
        }

    }
}

