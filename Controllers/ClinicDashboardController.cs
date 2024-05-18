using Microsoft.AspNetCore.Mvc;

namespace HMS.Controllers
{
    public class ClinicDashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
