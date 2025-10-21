using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SmartEXE.Models;

namespace SmartEXE.Pages.Admin
{
    [Authorize(Roles = "admin")] // ✅ Chỉ admin được vào
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
            if (!_context.Database.CanConnect())
            {
                TotalPartners = TotalUsers = TotalContent = TotalInteractions = 0;
                return;
            }

            TotalPartners = _context.Partners.Count();
            TotalUsers = _context.Users.Count();
            TotalContent = _context.Locations.Count() + _context.Topics.Count();
            TotalInteractions = _context.Analytics.Count();
        }
    }
}
