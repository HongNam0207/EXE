using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartEXE.Models;
using System.Collections.Generic;
using System.Linq;

namespace SmartEXE.Pages.Admin
{
    public class TourManageModel : PageModel
    {
        private readonly AilensContext _context;

        public List<Tour> Tours { get; set; } = new();

        [BindProperty]
        public Tour TourInput { get; set; } = new();

        public TourManageModel(AilensContext context)
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

            TourInput.Id = Guid.NewGuid();
            _context.Tours.Add(TourInput);
            _context.SaveChanges();
            return RedirectToPage();
        }

        public IActionResult OnPostDelete(Guid id)
        {
            var del = _context.Tours.Find(id);
            if (del != null)
            {
                _context.Tours.Remove(del);
                _context.SaveChanges();
            }
            return RedirectToPage();
        }

        private void LoadData()
        {
            Tours = _context.Tours.OrderByDescending(t => t.Title).ToList();
        }
    }
}
