using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartEXE.Models;
using System.Collections.Generic;
using System.Linq;

namespace SmartEXE.Pages.Admin
{
    public class TopicAdminModel : PageModel
    {
        private readonly AilensContext _context;

        public List<Topic> Topics { get; set; } = new();

        [BindProperty]
        public Topic TopicInput { get; set; } = new();

        public TopicAdminModel(AilensContext context)
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

            TopicInput.Id = Guid.NewGuid();
            _context.Topics.Add(TopicInput);
            _context.SaveChanges();

            return RedirectToPage();
        }

        public IActionResult OnPostDelete(Guid id)
        {
            var del = _context.Topics.Find(id);
            if (del != null)
            {
                _context.Topics.Remove(del);
                _context.SaveChanges();
            }
            return RedirectToPage();
        }

        private void LoadData()
        {
            Topics = _context.Topics.OrderByDescending(t => t.Name).ToList();
        }
    }
}
