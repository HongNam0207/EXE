using Microsoft.AspNetCore.Mvc;

namespace SmartCampusExplorer.Controllers
{
    public class LocationsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
