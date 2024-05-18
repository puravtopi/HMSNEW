using Microsoft.AspNetCore.Mvc;

namespace HMS.Controllers
{
    public class ConsultantDashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
