using ExternalLoginWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ExternalLoginWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : Controller
    {
        private readonly ExternalLoginDbContext _externalLoginDbContext;
        public EmployeeController(ExternalLoginDbContext externalLoginDbContext)
        {
            _externalLoginDbContext = externalLoginDbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
           var employees= await  _externalLoginDbContext.Employees.ToListAsync();
            return Ok(employees);
        }
    }
}
