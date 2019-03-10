using AccountingSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace AccountingSystem.Controllers
{
    public class AuthenticationController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            LoginModel defaultModel = new LoginModel();
            defaultModel.LoginError = "";
            defaultModel.PasswordError = "";
            
            return View(defaultModel);           
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {                                  
            return View(model);
        }
    }
}