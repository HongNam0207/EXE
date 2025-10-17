using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartEXE.Models;
using System.Collections.Generic;
using System.Linq;

namespace SmartEXE.Pages.Admin
{
    public class LocationAdminModel : PageModel
    {
        private readonly AilensContext _context;

        public List<Location> Locations { get; set; } = new();

        [BindProperty]
        public Location LocationInput { get; set; } = new();

        public LocationAdminModel(AilensContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            LoadData();
        }

        public IActionResult OnPostCreate()
        {
            if (!ModelState.IsValid) return Page();

            LocationInput.Id = Guid.NewGuid();
            LocationInput.CreatedAt = DateTime.Now;
            LocationInput.UpdatedAt = DateTime.Now;

            _context.Locations.Add(LocationInput);
            _context.SaveChanges();
            return RedirectToPage();
        }

        public IActionResult OnPostDelete(Guid id)
        {
            var del = _context.Locations.Find(id);
            if (del != null)
            {
                _context.Locations.Remove(del);
                _context.SaveChanges();
            }
            return RedirectToPage();
        }

        private void LoadData()
        {
            Locations = _context.Locations.OrderByDescending(x => x.CreatedAt).ToList();
        }
    }
}
