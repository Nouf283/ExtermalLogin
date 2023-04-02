using ExternalLoginWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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
        [Route("getEmployees")]
        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _externalLoginDbContext.Employees.ToListAsync();
            return Ok(employees);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee ([FromBody]Employee employee)
        {
           // employee.Id = (int)Guid.NewGuid();
            await _externalLoginDbContext.Employees.AddAsync(employee);
            await _externalLoginDbContext.SaveChangesAsync();
            return Ok(employee);
        }
    }
}
