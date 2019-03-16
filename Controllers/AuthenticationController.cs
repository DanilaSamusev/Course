using AccountingSystem.Models;
using AccountingSystem.Repository;
using Microsoft.AspNetCore.Mvc;
using AccountingSystem.Extensions;

namespace AccountingSystem.Controllers
{
    public class AuthenticationController : Controller
    {
        private UserRepository _userRepository { get; set; }

        public AuthenticationController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        
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
            string login = model.Login;

            User user = _userRepository.GetOne(login);
            HttpContext.Session.Set("user", user);

            return RedirectToAction("Menu","Menu");
            
            return View(model);
        }
    }
}