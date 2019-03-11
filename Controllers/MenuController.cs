using Microsoft.AspNetCore.Mvc;

namespace AccountingSystem.Controllers
{
    public class MenuController : Controller
    {
        [HttpGet]
        public IActionResult Menu()
        {
            return View();            
        }
                
    }
}