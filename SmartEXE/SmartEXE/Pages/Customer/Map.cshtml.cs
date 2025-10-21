using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartEXE.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace SmartEXE.Pages.Customer
{
    public class MapModel : PageModel
    {
        private readonly AilensContext _context;
        public List<Location> Locations { get; set; } = new();

        public MapModel(AilensContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            // ✅ Nếu chưa đăng nhập => chuyển ngay sang trang Login
            if (!User.Identity?.IsAuthenticated ?? false)
            {
                return Redirect("/Common/Login");
            }

            // ✅ Nếu đăng nhập nhưng role không phải user hoặc admin => vẫn chuyển về Login
            var role = User.FindFirstValue(ClaimTypes.Role);
            if (role != "user" && role != "admin")
            {
                return Redirect("/Common/Login");
            }

            // ✅ Nếu hợp lệ thì load dữ liệu bản đồ
            Locations = _context.Locations.ToList();
            return Page();
        }
    }
}
