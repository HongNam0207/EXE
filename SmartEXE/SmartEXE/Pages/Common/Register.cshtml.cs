using BCrypt.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SmartEXE.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SmartEXE.Pages.Common
{
    public class RegisterModel : PageModel
    {
        private readonly AilensContext _context;

        public RegisterModel(AilensContext context)
        {
            _context = context;
        }

        [BindProperty]
        public RegisterInput Input { get; set; } = new RegisterInput();

        public class RegisterInput
        {
            [Required(ErrorMessage = "Họ không được để trống")]
            public string FirstName { get; set; } = null!;

            [Required(ErrorMessage = "Tên không được để trống")]
            public string LastName { get; set; } = null!;

            [Required(ErrorMessage = "Email không được để trống")]
            [EmailAddress(ErrorMessage = "Email không hợp lệ")]
            public string Email { get; set; } = null!;

            [Required(ErrorMessage = "Mật khẩu không được để trống")]
            [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
            public string Password { get; set; } = null!;

            [Required(ErrorMessage = "Xác nhận mật khẩu không được để trống")]
            [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp")]
            public string ConfirmPassword { get; set; } = null!;
        }

        public void OnGet()
        {
        }

        // Đăng ký thường
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Kiểm tra trùng email
            if (await _context.Users.AnyAsync(u => u.Email == Input.Email))
            {
                ModelState.AddModelError(string.Empty, "Email đã tồn tại");
                return Page();
            }

            // Hash mật khẩu
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(Input.Password);

            var user = new User
            {
                Name = $"{Input.FirstName} {Input.LastName}",
                Email = Input.Email,
                PasswordHash = passwordHash,
                Role = "student",           // hoặc "User" cho mặc định
                LoginMethod = "Local",      // nên thống nhất chữ thường
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Sau khi đăng ký thành công, chuyển hướng về Login
            return RedirectToPage("/Common/Login");
        }

        // ========== ĐĂNG KÝ/LOGIN VỚI GOOGLE ==========

        /// <summary>
        /// Gửi request tới Google
        /// </summary>
        public IActionResult OnPostGoogle()
        {
            var redirectUrl = Url.Page("/Common/Register", "GoogleCallback");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// Callback từ Google
        /// </summary>
        public async Task<IActionResult> OnGetGoogleCallback()
        {
            // Claims đã có sẵn trong HttpContext.User
            var email = User.FindFirstValue(ClaimTypes.Email);
            var name = User.FindFirstValue(ClaimTypes.Name);
            var avatar = User.FindFirst("picture")?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return RedirectToPage("/Common/Login");
            }

            // Tìm user trong DB
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
                    Role = "student", // default role
                    CreatedAt = DateTime.UtcNow
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            // Đăng nhập hệ thống
            await SignInUser(user);

            return RedirectToPage("/Customer/Home");
        }

        /// <summary>
        /// Hàm helper để SignIn user
        /// </summary>
        private async Task SignInUser(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name ?? user.Email),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role ?? "User"),
                new Claim("LoginMethod", user.LoginMethod ?? "Local")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddDays(7)
                });
        }
    }
}
