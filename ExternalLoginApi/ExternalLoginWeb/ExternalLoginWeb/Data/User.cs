using Microsoft.AspNetCore.Identity;

namespace ExternalLoginWeb.Data
{
    public class User:IdentityUser
    {
        public string Password { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
    }
}
