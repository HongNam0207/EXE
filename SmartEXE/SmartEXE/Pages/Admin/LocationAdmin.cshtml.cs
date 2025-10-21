using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartEXE.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartEXE.Pages.Admin
{
    [Authorize(Roles = "admin")]
    public class LocationAdminModel : PageModel
    {
        private readonly AilensContext _context;

        public List<Location> Locations { get; set; } = new();

        [BindProperty]
        public Location LocationInput { get; set; } = new();

        public string? Message { get; set; }

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
            try
            {
                if (!ModelState.IsValid)
                {
                    Message = "❌ Invalid location data.";
                    LoadData();
                    return Page();
                }

                LocationInput.Id = Guid.NewGuid();
                LocationInput.CreatedAt = DateTime.Now;
                LocationInput.UpdatedAt = DateTime.Now;

                _context.Locations.Add(LocationInput);
                _context.SaveChanges();

                TempData["Message"] = "✅ Location added successfully.";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                Message = $"❌ Error creating location: {ex.Message}";
                LoadData();
                return Page();
            }
        }

        public IActionResult OnPostDelete(Guid id)
        {
            try
            {
                var del = _context.Locations.Find(id);
                if (del != null)
                {
                    _context.Locations.Remove(del);
                    _context.SaveChanges();
                    TempData["Message"] = "🗑️ Location deleted successfully.";
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"❌ Error deleting location: {ex.Message}";
            }

            return RedirectToPage();
        }

        private void LoadData()
        {
            Locations = _context.Locations
                .OrderByDescending(x => x.CreatedAt)
                .ToList();
        }
    }
}
