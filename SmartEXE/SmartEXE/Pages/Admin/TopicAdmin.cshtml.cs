using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartEXE.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartEXE.Pages.Admin
{
    [Authorize(Roles = "admin")] // ✅ Chỉ admin được truy cập
    public class TopicAdminModel : PageModel
    {
        private readonly AilensContext _context;

        public List<Topic> Topics { get; set; } = new();

        [BindProperty]
        public Topic TopicInput { get; set; } = new();

        public string? Message { get; set; }

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
            try
            {
                if (!ModelState.IsValid)
                {
                    Message = "❌ Invalid topic data.";
                    LoadData();
                    return Page();
                }

                TopicInput.Id = Guid.NewGuid();
                _context.Topics.Add(TopicInput);
                _context.SaveChanges();

                TempData["Message"] = "✅ Topic created successfully.";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                Message = $"❌ Error creating topic: {ex.Message}";
                LoadData();
                return Page();
            }
        }

        public IActionResult OnPostDelete(Guid id)
        {
            try
            {
                var del = _context.Topics.Find(id);
                if (del != null)
                {
                    _context.Topics.Remove(del);
                    _context.SaveChanges();
                    TempData["Message"] = "🗑️ Topic deleted successfully.";
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"❌ Error deleting topic: {ex.Message}";
            }

            return RedirectToPage();
        }

        private void LoadData()
        {
            Topics = _context.Topics.OrderByDescending(t => t.Name).ToList();
        }
    }
}
