using FPTPlay.Data;
using FPTPlay.Models;
using FPTPlay.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using FPTPlay.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Http.HttpResults;
using FPTPlay.Admin.Models;

namespace FPTPlay.Services
{
    public class UserService
    {
        private readonly FPTPlayContext _db;

        public UserService(FPTPlayContext db)
        {
            _db = db;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _db.Users
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.Id)
                .ToListAsync();
        }
        public async Task<User?> GetByIdAsync(int id)
        {
            return await _db.Users
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task<User?> GetByPhoneAsync(string phone)
            => await _db.Users.FirstOrDefaultAsync(x => x.Mobile == phone);

        public async Task<User?> GetByEmailAsync(string email)
            => await _db.Users.FirstOrDefaultAsync(x => x.Email == email);

        public async Task AddAsync(User user)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }

        // 🔐 LOGIN
        public async Task<User?> LoginAsync(string phone, string password)
        {
            var user = await _db.Users
                .FirstOrDefaultAsync(u => u.Mobile == phone && u.IsActive && !u.IsDeleted);

            if (user == null) return null;

            bool isValid = Hash.Verify(password, user.PasswordHash);
            return isValid ? user : null;
        }

        public async Task<User?> GetCurrentUserAsync(ClaimsPrincipal principal)
        {
            var phone = principal.Identity?.Name;
            if (string.IsNullOrEmpty(phone)) return null;

            return await _db.Users.FirstOrDefaultAsync(x => x.Mobile == phone);
        }

        // 🔍 Search + Paging
        public async Task<(List<User> Users, int TotalCount)> GetUsersAsync(
            int page = 1,
            int pageSize = 10,
            string search = "")
        {
            var query = _db.Users.Where(u => !u.IsDeleted).AsQueryable(); 

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(u =>
                    u.Mobile.Contains(search) ||
                    u.FullName.Contains(search));
            }

            var totalCount = await query.CountAsync();

            var users = await query
                .OrderByDescending(u => u.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (users, totalCount);
        }

        // ➕ CREATE
        public async Task<(bool Success, string Error)> CreateAsync(UserCreateViewModel model)
        {
            if (await _db.Users.AnyAsync(u => u.Mobile == model.Mobile && !u.IsDeleted))
                return (false, "Số điện thoại đã tồn tại !");

            if (await _db.Users.AnyAsync(u => u.Email == model.Email && !u.IsDeleted))
                return (false, "Email này đã tồn tại !");

            var user = new User
            {
                Mobile = model.Mobile,
                Email = model.Email,
                FullName = model.FullName,
                Password = model.Password,
                PasswordHash = Hash.HashPassword(model.Password),
                AvatarUrl = model.AvatarUrl,
                Role = model.Role,
                IsActive = model.IsActive,
                IsDeleted = false,
                CreatedAt = DateTime.Now
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return (true,"");
        }        

        // ✏️ UPDATE
        public async Task<(bool Success, string Error)> UpdateAsync(UserEditViewModel model)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == model.Id && !u.IsDeleted);
            if (user == null) return (false, "User không tồn tại");

            if (await _db.Users.AnyAsync(u => u.Mobile == model.Mobile && u.Id != model.Id))
                return (false, "Số điện thoại đã tồn tại");

            if (await _db.Users.AnyAsync(u => u.Email == model.Email && u.Id != model.Id))
                return (false, "Email đã tồn tại");

            user.Mobile = model.Mobile;
            user.Email = model.Email;
            user.FullName = model.FullName;
            user.AvatarUrl = model.AvatarUrl;
            user.Role = model.Role;
            user.IsActive = model.IsActive;
            //user.UpdatedBy 

            if (!string.IsNullOrWhiteSpace(model.NewPassword))
            {
                user.PasswordHash = Hash.HashPassword(model.NewPassword);
            }

            await _db.SaveChangesAsync();
            return (true, "");
        }        

        // ❌ SOFT DELETE
        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
            if (user == null) return false;           

            if (user.Role == "Admin") return false; //Chặn ko cho xóa user có Role là "Admin"

            user.IsActive = false;
            user.IsDeleted = true;
            await _db.SaveChangesAsync();
            return true;
        }        

        // 🔄 ACTIVE
        public async Task<bool> ActivateAsync(int id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
            if (user == null) return false;

            user.IsActive = true;
            user.IsDeleted = false;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}