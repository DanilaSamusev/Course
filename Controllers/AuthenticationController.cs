using AccountingSystem.Models;
using AccountingSystem.Repository;
using Microsoft.AspNetCore.Mvc;
using AccountingSystem.Extensions;

namespace AccountingSystem.Controllers
{
    public class AuthenticationController : Controller
    {
        private IUserRepository _userRepository { get; set; }

        public AuthenticationController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult Login()
        {           
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            string login = model.Login;
            int password = model.Password;

            User user = _userRepository.GetOneByLoginAndPassword(login, password);

            if (user == null)
            {
                model.AuthenticationError = "Пользователь не найден!";
                return View(model);
            }

            HttpContext.Session.Set("user", user);

            return RedirectToAction("Menu", "Menu");
        }
    }
}