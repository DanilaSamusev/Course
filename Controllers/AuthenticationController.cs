using AccountingSystem.Models;
using AccountingSystem.Repository;
using Microsoft.AspNetCore.Mvc;
using AccountingSystem.Extensions;
using FluentValidation;
using FluentValidation.Results;

namespace AccountingSystem.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly AbstractValidator<LoginModel> _loginModelValidator;

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
                return View("~/Views/Error400.cshtml");
            }
                        
            User user = _userRepository.GetOneByLoginAndPassword(model);

            if (user == null)
            {
                model.AuthenticationError = "Логин или пароль введены неверно!";
                return View(model);
            }

            HttpContext.Session.Set("user", user);

            return RedirectToAction("Menu", "Menu");
        }
    }
}