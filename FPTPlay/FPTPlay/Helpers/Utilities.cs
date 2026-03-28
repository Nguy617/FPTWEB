using System.Text;
using System.Security.Cryptography;

namespace FPTPlay.Helpers
{
    public static class Parameters
    {
        public const string imageFolder = "";
    }
   
    public static class CurrentUser
    {
        public const string UserId = "UserId";
        public const string Username = "Username";
        public const string Role = "Role";
    }

    // Hash password SHA256 (demo)
    public static class Hash
    {
        public static string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return string.Empty;

            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        public static bool Verify(string inputPassword, string dbHash)
        {
            return HashPassword(inputPassword) == dbHash;
        }
    }

    public static class SlugHelper
    {
        public static string GenerateSlug(string phrase)
        {
            string str = phrase.ToLower();

            str = str.Replace("đ", "d");

            str = System.Text.RegularExpressions.Regex.Replace(str, @"[^a-z0-9\s-]", "");

            str = System.Text.RegularExpressions.Regex.Replace(str, @"\s+", " ").Trim();

            str = str.Replace(" ", "-");

            return str;
        }
    }

}
