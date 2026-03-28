using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FPTPlay.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(10)]
        public string Mobile { get; set; }

        [StringLength(100)]
        public string? Email { get; set; }

        [MaxLength(100)]
        public string Password { get; set; }

        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; }

        [MaxLength(100)]
        public string FullName { get; set; }
        public string? AvatarUrl { get; set; }  

        [Required]
        [MaxLength(20)]
        public string Role { get; set; } = "User"; // Admin / User
        public bool IsActive { get; set; } = true;//mặc định là hoạt động        
        public bool IsDeleted { get; set; } = false;//mặc định là ko xóa        
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int? DeletedBy { get; set; }
    }
}
