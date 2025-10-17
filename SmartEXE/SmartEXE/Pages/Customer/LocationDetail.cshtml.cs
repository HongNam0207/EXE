using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartEXE.Models;

namespace SmartEXE.Pages.Customer
{
    public class LocationDetailModel : PageModel
    {
        private readonly AilensContext _context;

        public LocationDetailModel(AilensContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Location Location { get; set; } = default!;

        public IActionResult OnGet(Guid id)
        {
            Location = _context.Locations.FirstOrDefault(l => l.Id == id);
            if (Location == null)
                return NotFound();

            return Page();
        }
    }
}
