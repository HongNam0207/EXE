using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartEXE.Models; // namespace đúng theo project của bạn
using System.Linq;

namespace SmartEXE.Pages
{
    public class TestDBModel : PageModel
    {
        private readonly AilensContext _context;
        public int UserCount { get; set; }
        public int LocationCount { get; set; }

        public TestDBModel(AilensContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            UserCount = _context.Users.Count();
            LocationCount = _context.Locations.Count();
        }
    }
}
