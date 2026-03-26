using Microsoft.AspNetCore.Mvc;

namespace FPTPlay.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
