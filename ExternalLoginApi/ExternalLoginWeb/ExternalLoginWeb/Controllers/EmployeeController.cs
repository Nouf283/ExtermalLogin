using Microsoft.AspNetCore.Mvc;

namespace ExternalLoginWeb.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
