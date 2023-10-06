using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LacDau.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class UserManagementController : Controller
    {
        public IActionResult Manager()
        {
            return View();
        }
        public IActionResult Customer()
        {
            return View();
        }
    }
}
