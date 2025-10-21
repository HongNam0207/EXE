using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SmartEXE.Models;
using System.Collections.Generic;
using System.Linq;

namespace SmartEXE.Pages.Admin
{
    [Authorize(Roles = "admin")]
    public class AnalyticsModel : PageModel
    {
        private readonly AilensContext _context;

        public List<Analytic> Analytics { get; set; } = new();
        public int TotalViews { get; set; }
        public int TotalSearches { get; set; }
        public int TotalNavigations { get; set; }

        public AnalyticsModel(AilensContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            try
            {
                Analytics = _context.Analytics
                    .Include(a => a.User)
                    .Include(a => a.Location)
                    .OrderByDescending(a => a.Timestamp)
                    .Take(100)
                    .ToList();

                TotalViews = _context.Analytics.Count(a => a.Action == "view" || a.Action == "view_location");
                TotalSearches = _context.Analytics.Count(a => a.Action == "search");
                TotalNavigations = _context.Analytics.Count(a => a.Action == "navigate");
            }
            catch
            {
                Analytics = new();
                TotalViews = TotalSearches = TotalNavigations = 0;
            }
        }
    }
}
