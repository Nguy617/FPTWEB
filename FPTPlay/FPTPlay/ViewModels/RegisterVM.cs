using System.ComponentModel.DataAnnotations;

namespace FPTPlay.ViewModels
{
    public class RegisterVM
    {
        [Required]
        [MaxLength(10)]
        public string Mobile { get; set; }

        [MaxLength(255)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu không khớp !")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string FullName { get; set; }

        [MaxLength(255)]
        public string? AvatarUrl { get; set; }
    }
}
