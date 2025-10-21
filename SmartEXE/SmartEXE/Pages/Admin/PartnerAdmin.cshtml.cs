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
    public class PartnerAdminModel : PageModel
    {
        private readonly AilensContext _context;

        public List<Partner> Partners { get; set; } = new();

        [BindProperty]
        public Partner PartnerInput { get; set; } = new();

        public string? Message { get; set; }

        public PartnerAdminModel(AilensContext context)
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
                    Message = "❌ Invalid partner data.";
                    LoadData();
                    return Page();
                }

                PartnerInput.Id = Guid.NewGuid();
                PartnerInput.CreatedAt = DateTime.Now;
                _context.Partners.Add(PartnerInput);
                _context.SaveChanges();

                TempData["Message"] = "✅ Partner created successfully!";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                Message = $"❌ Error creating partner: {ex.Message}";
                LoadData();
                return Page();
            }
        }

        public IActionResult OnPostEdit(Guid id)
        {
            try
            {
                var existing = _context.Partners.Find(id);
                if (existing != null)
                {
                    existing.Name = PartnerInput.Name;
                    existing.Type = PartnerInput.Type;
                    existing.ContactEmail = PartnerInput.ContactEmail;
                    existing.Phone = PartnerInput.Phone;
                    existing.Website = PartnerInput.Website;
                    existing.Description = PartnerInput.Description;
                    //existing.UpdatedAt = DateTime.Now;

                    _context.SaveChanges();
                    TempData["Message"] = "✏️ Partner updated successfully!";
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"❌ Error editing partner: {ex.Message}";
            }

            return RedirectToPage();
        }

        public IActionResult OnPostDelete(Guid id)
        {
            try
            {
                var del = _context.Partners.Find(id);
                if (del != null)
                {
                    _context.Partners.Remove(del);
                    _context.SaveChanges();
                    TempData["Message"] = "🗑️ Partner deleted successfully.";
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"❌ Error deleting partner: {ex.Message}";
            }

            return RedirectToPage();
        }

        private void LoadData()
        {
            Partners = _context.Partners
                .OrderByDescending(x => x.CreatedAt)
                .ToList();
        }
    }
}
