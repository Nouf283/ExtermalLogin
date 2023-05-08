using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExternalLoginWeb.Data
{
    //public class ExternalLoginDbContext:DbContext
    //{
    //    public ExternalLoginDbContext(DbContextOptions options) : base(options)
    //    {

    //    }
    //    public DbSet<Employee> Employees { get; set; }
    //    public DbSet<User> Users { get; set; }
    //}
    public class ExternalLoginDbContext : IdentityDbContext
    {
        public ExternalLoginDbContext(DbContextOptions<ExternalLoginDbContext> options) : base(options)
        {

        }
        //public DbSet<Employee> Employees { get; set; }
        //public DbSet<User> Users { get; set; }
    }
}
