using Microsoft.AspNetCore.Mvc;

namespace SmartCampusExplorer.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
