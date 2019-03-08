using Microsoft.AspNetCore.Mvc;

namespace AccountingSystem.Controllers
{
    public class AuthenticationController : Controller
    {
        
        public IActionResult Login()
        {
            return View();           
        }
    }
}