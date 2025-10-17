using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SmartEXE.Pages.Common
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string? ErrorMessage { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            // Ví dụ kiểm tra đơn giản – bạn có thể thay bằng DB check
            if (Email == "admin@fpt.edu.vn" && Password == "123456")
            {
                // TODO: Lưu session nếu cần
                return RedirectToPage("/Customer/Home");
            }

            ErrorMessage = "Sai tài khoản hoặc mật khẩu.";
            return Page();
        }
    }
}
