using LacDau.Data;
using LacDau.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LacDau.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class PaymentMomoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMomoService _momoService;

        public PaymentMomoController(ApplicationDbContext context, IMomoService momo)
        {
            _context = context;
            _momoService = momo;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
