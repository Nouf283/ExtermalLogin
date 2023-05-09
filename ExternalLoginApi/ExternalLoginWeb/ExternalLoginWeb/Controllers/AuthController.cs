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
using Microsoft.AspNetCore.Identity;
using ExternalLoginWeb.Migrations;

namespace ExternalLoginWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly ExternalLoginDbContext _externalLoginDbContext;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<User> signInManager;

        public AuthController(ExternalLoginDbContext externalLoginDbContext, IConfiguration configuration, SignInManager<User> signInManager)
        {
            _externalLoginDbContext = externalLoginDbContext;
            this._configuration = configuration;
            this.signInManager = signInManager;

        }

        [BindProperty]
        public IEnumerable<AuthenticationScheme> ExternalLoginProviders { get; set; }

        public async Task OnGetAsync()
        {
            this.ExternalLoginProviders = await signInManager.GetExternalAuthenticationSchemesAsync();
        }

        public IActionResult OnPostLoginExternally(string provider)
        {
            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, null);
            properties.RedirectUri = Url.Action("ExternalLoginCallback", "Auth");

            return Challenge(properties, provider);
        }

        public async Task<IActionResult> ExternalLoginCallback()
        {
            var loginInfo = await signInManager.GetExternalLoginInfoAsync();
            var emailClaim = loginInfo.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
            var userClaim = loginInfo.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);

            if (emailClaim != null && userClaim != null)
            {
                var user = new User { Email = emailClaim.Value, UserName = userClaim.Value };
                await signInManager.SignInAsync(user, false);
                return Ok(user);
            }
            else
            {
                ModelState.AddModelError("UnAuthorized", "You are not allowed to access the endpoint");
                return  Unauthorized(ModelState);
            }

            //   return RedirectToPage("/Index");
            //return Ok(user);
        }

        [HttpPost]
        [Route("adduser")]
        [AllowAnonymous]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            // employee.Id = (int)Guid.NewGuid();
            //await _externalLoginDbContext.Users.AddAsync(user);
            //await _externalLoginDbContext.SaveChangesAsync();
            //return Ok(user);
            return null;
        }

        [HttpPost]
        [Route("authenticateuser")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] User credential)
        {
            //var user = await _externalLoginDbContext.Users.Where(x => x.UserName == credential.UserName && x.Password == credential.Password).FirstOrDefaultAsync();
            //if (user != null)
            //{
            //    var claims = new List<Claim>
            //        {
            //            new Claim(ClaimTypes.Name,user.UserName),
            //            new Claim(ClaimTypes.Email,user.Email),
            //        };
            //    var expiresAt = DateTime.UtcNow.AddMinutes(10);
            //    return Ok(new
            //    {
            //        access_token = CreateToken(claims, expiresAt),
            //        expires_at = expiresAt
            //    });
            //}

            //ModelState.AddModelError("UnAuthorized", "You are not allowed to access the endpoint");
            //return  Unauthorized(ModelState);

            return null;
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

