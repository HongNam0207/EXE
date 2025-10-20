using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SmartEXE.Models;

namespace SmartEXE.Pages.Customer
{
    public class ProfileModel : PageModel
    {
        private readonly AilensContext _context;

        public ProfileModel(AilensContext context)
        {
            _context = context;
        }

        [BindProperty]
        public User Profile { get; set; } = new User();

        [BindProperty]
        public ChangePasswordDTO ChangePassword { get; set; } = new ChangePasswordDTO();

        [TempData]
        public string? SuccessMessage { get; set; }

        // ========== LOAD PROFILE ==========
        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return RedirectToPage("/Common/Login");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            if (user == null)
                return RedirectToPage("/Common/Login");

            Profile = user;
            return Page();
        }

        // ========== CẬP NHẬT PROFILE ==========
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return RedirectToPage("/Common/Login");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            if (user == null)
                return RedirectToPage("/Common/Login");

            user.Name = Profile.Name;
            user.AvatarUrl = Profile.AvatarUrl;

            await _context.SaveChangesAsync();

            SuccessMessage = "Cập nhật thông tin thành công!";
            return RedirectToPage();
        }

        // ========== ĐỔI MẬT KHẨU ==========
        public async Task<IActionResult> OnPostChangePasswordAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return RedirectToPage("/Common/Login");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            if (user == null)
                return RedirectToPage("/Common/Login");

            // Kiểm tra mật khẩu cũ
            if (string.IsNullOrEmpty(user.PasswordHash) || !BCrypt.Net.BCrypt.Verify(ChangePassword.OldPassword, user.PasswordHash))
            {
                ModelState.AddModelError("ChangePassword.OldPassword", "Mật khẩu cũ không đúng.");
                Profile = user;
                return Page();
            }

            // Kiểm tra mật khẩu mới và xác nhận
            if (ChangePassword.NewPassword != ChangePassword.ConfirmPassword)
            {
                ModelState.AddModelError("ChangePassword.ConfirmPassword", "Mật khẩu xác nhận không khớp.");
                Profile = user;
                return Page();
            }

            // Cập nhật mật khẩu mới
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(ChangePassword.NewPassword);
            await _context.SaveChangesAsync();

            SuccessMessage = "Đổi mật khẩu thành công!";
            return RedirectToPage();
        }
    }

    // DTO đổi mật khẩu với DataAnnotation để hiện lỗi dưới label
    public class ChangePasswordDTO
    {
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu cũ")]
        public string OldPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới")]
        [MinLength(6, ErrorMessage = "Mật khẩu mới phải ít nhất 6 ký tự")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu mới")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
