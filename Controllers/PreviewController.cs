using Microsoft.AspNetCore.Mvc;

namespace AccountingSystem.Controllers
{
    public class PreviewController : Controller
    {
        [HttpGet]
        public IActionResult Preview()
        {
            return View();
        }
    }
}