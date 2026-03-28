using FPTPlay.Models;
using System.ComponentModel.DataAnnotations;

namespace FPTPlay.Admin.Models
{
    // ================= CREATE =================
    public class UserCreateViewModel
    {
        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        [MaxLength(10)]
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "SĐT không hợp lệ")]
        [Display(Name = "Số điện thoại")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [MaxLength(255)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Mật khẩu tối thiểu 6 ký tự")]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; } = "123456";//mặc định

        [Required(ErrorMessage = "Xác nhận mật khẩu")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu không khớp")]
        [Display(Name = "Xác nhận mật khẩu")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        [MaxLength(100)]
        [Display(Name = "Họ tên")]
        public string FullName { get; set; }

        [Display(Name = "Avatar")]
        public string? AvatarUrl { get; set; }

        [Required(ErrorMessage = "Vai trò là bắt buộc")]
        [Display(Name = "Vai trò")]
        public string Role { get; set; } = "Customer";

        [Display(Name = "Kích hoạt")]
        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;
    }

    // ================= EDIT =================
    public class UserEditViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(10)]
        [Display(Name = "Số điện thoại")]
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "SĐT không hợp lệ")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [MaxLength(255)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới (để trống nếu không đổi)")]
        public string? NewPassword { get; set; }

        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        [MaxLength(100)]
        [Display(Name = "Họ tên")]
        public string FullName { get; set; }

        [Display(Name = "Avatar")]
        public string? AvatarUrl { get; set; }

        [Required(ErrorMessage = "Vai trò là bắt buộc")]
        [Display(Name = "Vai trò")]
        public string Role { get; set; }

        [Display(Name = "Kích hoạt")]
        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }
    }

    // ================= LIST =================
    public class UserListViewModel
    {
        public int Id { get; set; }

        public string Mobile { get; set; }

        public string? Email { get; set; }
        public string FullName { get; set; }

        public string? AvatarUrl { get; set; }

        public string Role { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class UserListViewModel2
    {
        public List<User> Users { get; set; }
        public string? Search { get; set; }
        public string Role { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
    }
}