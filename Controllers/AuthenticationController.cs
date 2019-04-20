using AccountingSystem.Models;
using AccountingSystem.Repository;
using Microsoft.AspNetCore.Mvc;
using AccountingSystem.Extensions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;

namespace AccountingSystem.Controllers
{
    public class AuthenticationController : Controller
    {
        private IUserRepository _userRepository { get; set; }
        private AbstractValidator<LoginModel> _loginModelValidator { get; set; }

        public AuthenticationController(IUserRepository userRepository,
            AbstractValidator<LoginModel> loginModelValidator)
        {
            _userRepository = userRepository;
            _loginModelValidator = loginModelValidator;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            ValidationResult result = _loginModelValidator.Validate(model);

            if (!result.IsValid)
            {
                return View("~/Views/Error403.cshtml");
            }
            
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