using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using FPTPlay.Data;
using System.Linq;
using FPTPlay.ViewModels;
using FPTPlay.Services;
using FPTPlay.Models;
using FPTPlay.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace FPTPlay.Controllers
{
    public class AccountController : Controller
    {
        private readonly FPTPlayContext _context;
        private readonly UserService _userService;

        public AccountController(FPTPlayContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }

        // ================= REGISTER =================
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var existing = await _userService.GetByEmailAsync(model.Email);
            if (existing != null)
            {
                ModelState.AddModelError("", "Email này đã tồn tại");
                return View(model);
            }

            var user = new User
            {
                Mobile = model.Mobile,
                Email = model.Email,
                FullName = model.FullName,
                Password = model.Password,
                PasswordHash = Hash.HashPassword(model.Password),
                Role = "Customer",
                IsActive = true,
                IsDeleted = false,
                CreatedAt = DateTime.Now
            };

            await _userService.AddAsync(user);

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Role, user.Role ?? "Customer"),
            new Claim("Avatar", user.AvatarUrl ?? "")
        };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal);

                // 🔥 Redirect đúng Area
                if (user.Role == "Admin")
                {
                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                }

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Email hoặc mật khẩu không chính xác.";
            return View();
        }
        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

    }
}
