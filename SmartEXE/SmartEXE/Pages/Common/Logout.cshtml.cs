using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SmartEXE.Pages.Common
{
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnPost()
        {
            // Xóa cookie login
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Điều hướng về trang Home
            return RedirectToPage("/Customer/Home");
        }

        // Nếu người dùng vào /Common/Logout trực tiếp thì cũng logout
        public async Task<IActionResult> OnGet()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("/Customer/Home");
        }

    }
}
