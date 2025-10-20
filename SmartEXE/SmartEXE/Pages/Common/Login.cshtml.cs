using System.Security.Claims;
using BCrypt.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SmartEXE.Models;

namespace SmartEXE.Pages.Common
{
    public class LoginModel : PageModel
    {
        private readonly AilensContext _context;

        public LoginModel(AilensContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        public string? ErrorMessage { get; set; }

        public void OnGet() { }

        /// <summary>
        /// Đăng nhập bằng Email/Password
        /// </summary>
        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                ErrorMessage = "Vui lòng nhập đầy đủ Email và Mật khẩu.";
                return Page();
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == Email && u.LoginMethod == "Local");
            if (user != null && !string.IsNullOrEmpty(user.PasswordHash)
                 && BCrypt.Net.BCrypt.Verify(Password, user.PasswordHash))
            {
                await SignInUser(user);
                return RedirectToPage("/Customer/Home");
            }


            ErrorMessage = "Sai tài khoản hoặc mật khẩu.";
            return Page();
        }

        /// <summary>
        /// Bắt đầu login Google
        /// </summary>
        public IActionResult OnPostGoogle()
        {
            // callback về handler OnGetGoogleCallback
            var redirectUrl = Url.Page("/Common/Login", "GoogleCallback");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, "Google");
        }

        /// <summary>
        /// Google callback
        /// </summary>
        public async Task<IActionResult> OnGetGoogleCallback()
        {
            // Sau khi Google xác thực, claims sẽ có trong HttpContext.User
            var email = User.FindFirstValue(ClaimTypes.Email);
            var name = User.FindFirstValue(ClaimTypes.Name);
            var avatar = User.FindFirst("picture")?.Value;

            if (string.IsNullOrEmpty(email))
            {
                ErrorMessage = "Không lấy được email từ Google.";
                return RedirectToPage("/Common/Login");
            }

            // Tìm hoặc tạo user mới
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                user = new User
                {
                    Id = Guid.NewGuid(),
                    Name = !string.IsNullOrEmpty(name) ? name : email,
                    Email = email,
                    AvatarUrl = avatar,
                    LoginMethod = "Google",
                    Role = "student",
                    CreatedAt = DateTime.UtcNow
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            await SignInUser(user);
            return RedirectToPage("/Customer/Home");
        }

        /// <summary>
        /// Helper: tạo session bằng cookie
        /// </summary>
        private async Task SignInUser(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name ?? ""),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role ?? "User"),
                new Claim("LoginMethod", user.LoginMethod ?? "email")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true, // duy trì login
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(12) // hết hạn sau 12h
                });
        }
    }
}
