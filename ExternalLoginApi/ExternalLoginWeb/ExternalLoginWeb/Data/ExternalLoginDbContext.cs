using Microsoft.EntityFrameworkCore;

namespace ExternalLoginWeb.Data
{
    public class ExternalLoginDbContext:DbContext
    {
        public ExternalLoginDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Employee> Employees { get; set; }
    }
}
