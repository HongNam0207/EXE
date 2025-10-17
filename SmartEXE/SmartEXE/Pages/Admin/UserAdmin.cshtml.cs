using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SmartEXE.Models;
using System.Linq;

namespace SmartEXE.Pages.Admin
{
    public class UserAdminModel : PageModel
    {
        private readonly AilensContext _context;

        public int TotalPartners { get; set; }
        public int TotalUsers { get; set; }
        public int TotalContent { get; set; }
        public int TotalInteractions { get; set; }

        public UserAdminModel(AilensContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            try
            {
                // Kiểm tra có kết nối DB không
                if (_context.Database.CanConnect())
                {
                    // ✅ Lấy trực tiếp dữ liệu từ các bảng
                    TotalPartners = _context.Partners.Count();
                    TotalUsers = _context.Users.Count();
                    TotalContent = _context.Locations.Count() + _context.Topics.Count();
                    TotalInteractions = _context.Analytics.Count();
                }
                else
                {
                    // Nếu không kết nối được
                    TotalPartners = TotalUsers = TotalContent = TotalInteractions = 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading dashboard: " + ex.Message);
                TotalPartners = TotalUsers = TotalContent = TotalInteractions = 0;
            }
        }


    }
}
