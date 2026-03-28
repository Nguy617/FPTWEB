using FPTPlay.Admin.Models;
using FPTPlay.Helpers;
using FPTPlay.Models;
using FPTPlay.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FPTPlay.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        // ================= LIST + SEARCH + PAGING =================
        public async Task<IActionResult> Index(int page = 1, string search = "")
        {
            int pageSize = 10;

            var (users, totalCount) = await _userService.GetUsersAsync(page, pageSize, search);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            ViewBag.Search = search;

            return View(users);
        }

        // ================= CREATE =================
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _userService.CreateAsync(model);

            if (!result.Success)
            {
                ModelState.AddModelError("Error", result.Error);
                return View(model);
            }

            TempData["Success"] = "Tạo user thành công!";
            return RedirectToAction(nameof(Index));
        }                

        // ================= EDIT =================
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();

            // Map sang ViewModel
            var vm = new UserEditViewModel
            {
                Id = user.Id,
                Mobile = user.Mobile,
                Email = user.Email,
                FullName = user.FullName,
                AvatarUrl = user.AvatarUrl,
                Role = user.Role,
                IsActive = user.IsActive
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _userService.UpdateAsync(model);

            if (!result.Success)
            {
                ModelState.AddModelError("Mobile", result.Error);
                return View(model);
            }

            TempData["Success"] = "Cập nhật user thành công!";
            return RedirectToAction(nameof(Index));
        }

        // ================= DELETE =================
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await _userService.DeleteAsync(id);

            if (!success)
            {
                TempData["Error"] = "Không thể xóa user!";
            }
            else
            {
                TempData["Success"] = "Xóa user thành công!";
            }

            return RedirectToAction(nameof(Index));
        }

        // ================= ACTIVATE =================
        public async Task<IActionResult> Activate(int id)
        {
            var success = await _userService.ActivateAsync(id);

            if (!success)
                TempData["Error"] = "Không thể kích hoạt user!";
            else
                TempData["Success"] = "Kích hoạt user thành công!";

            return RedirectToAction(nameof(Index));
        }
    }
}