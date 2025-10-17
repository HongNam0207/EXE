using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartEXE.Models;
using System.Collections.Generic;
using System.Linq;

namespace SmartEXE.Pages.Admin
{
    public class PartnerAdminModel : PageModel
    {
        private readonly AilensContext _context;

        public List<Partner> Partners { get; set; } = new();

        [BindProperty]
        public Partner PartnerInput { get; set; } = new();

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
            if (!ModelState.IsValid) return Page();

            PartnerInput.Id = Guid.NewGuid();
            PartnerInput.CreatedAt = DateTime.Now;

            _context.Partners.Add(PartnerInput);
            _context.SaveChanges();
            return RedirectToPage();
        }

        public IActionResult OnPostEdit(Guid id)
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
                _context.SaveChanges();
            }
            return RedirectToPage();
        }

        public IActionResult OnPostDelete(Guid id)
        {
            var del = _context.Partners.Find(id);
            if (del != null)
            {
                _context.Partners.Remove(del);
                _context.SaveChanges();
            }
            return RedirectToPage();
        }

        private void LoadData()
        {
            Partners = _context.Partners.OrderByDescending(x => x.CreatedAt).ToList();
        }
    }
}
