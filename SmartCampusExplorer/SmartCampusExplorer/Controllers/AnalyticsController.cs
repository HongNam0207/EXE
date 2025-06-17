using Microsoft.AspNetCore.Mvc;

namespace SmartCampusExplorer.Controllers
{
    public class AnalyticsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
