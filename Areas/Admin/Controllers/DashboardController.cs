using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LacDau.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            ViewData["CurrentPage"] = "Dashboard";
            return View();
        }
    }
}
