using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartEXE.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartEXE.Pages.Admin
{
    [Authorize(Roles = "admin")] // ✅ Chỉ admin mới được truy cập
    public class TourManageModel : PageModel
    {
        private readonly AilensContext _context;

        public List<Tour> Tours { get; set; } = new();

        [BindProperty]
        public Tour TourInput { get; set; } = new();

        public string? Message { get; set; } // ✅ thông báo lỗi hoặc thành công

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
            try
            {
                if (!ModelState.IsValid)
                {
                    Message = "❌ Invalid tour data.";
                    LoadData();
                    return Page();
                }

                TourInput.Id = Guid.NewGuid();
                //TourInput.CreatedAt = DateTime.Now; // ✅ nếu có cột thời gian
                _context.Tours.Add(TourInput);
                _context.SaveChanges();

                TempData["Message"] = "✅ Tour created successfully!";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                Message = $"❌ Error creating tour: {ex.Message}";
                LoadData();
                return Page();
            }
        }

        public IActionResult OnPostDelete(Guid id)
        {
            try
            {
                var del = _context.Tours.Find(id);
                if (del == null)
                {
                    TempData["Message"] = "⚠️ Tour not found.";
                    return RedirectToPage();
                }

                _context.Tours.Remove(del);
                _context.SaveChanges();

                TempData["Message"] = "🗑️ Tour deleted successfully.";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"❌ Error deleting tour: {ex.Message}";
                return RedirectToPage();
            }
        }

        private void LoadData()
        {
            Tours = _context.Tours
    .OrderBy(t => t.Title) // sắp theo tên cho dễ nhìn
    .ToList();

        }
    }
}
