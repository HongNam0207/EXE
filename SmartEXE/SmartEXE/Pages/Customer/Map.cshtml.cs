using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartEXE.Models;
using System.Collections.Generic;
using System.Linq;

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

        public void OnGet()
        {
            Locations = _context.Locations.ToList();
        }
    }
}
