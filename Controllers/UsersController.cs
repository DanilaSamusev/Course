using System.Collections.Generic;
using System.Linq;
using AccountingSystem.Repository;
using Microsoft.AspNetCore.Mvc;
using AccountingSystem.Extensions;
using AccountingSystem.Models;
using AccountingSystem.Services;
using FluentValidation;
using FluentValidation.Results;

namespace AccountingSystem.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly Validator _validator;
        private readonly AbstractValidator<User> _userValidator;
        private const string Users_ = "users";
        private const string UserExists = "Пользователь с таким логином уже существует!";

        public UsersController(IUserRepository userRepository, Validator validator,
            AbstractValidator<User> userValidator)
        {
            _userRepository = userRepository;
            _validator = validator;
            _userValidator = userValidator;
        }

        public IActionResult Users()
        {
            IActionResult result = CheckUserAccess();
            
            if (result != null)
            {
                return result;
            }
                                   
            List<User> users = GetUsers();
                     
            return View(users);
        }

        public IActionResult DeleteUser(long userId)
        {            
            IActionResult result = CheckUserAccess();
            
            if (result != null)
            {
                return result;
            }

            if (userId <= 0)
            {
                return View("~/Views/Error400.cshtml");
            }
            
            List<User> users = GetUsers();

            _userRepository.DeleteOneById(userId);
            User user = users.FirstOrDefault(u => u.Id == userId);
            users.Remove(user);
            HttpContext.Session.Set(Users_, users);

            return View("UsersActionResult", "Данные успешно удалёны");
        }

        public IActionResult ModifyUser(User user)
        {           
            IActionResult actionResult = CheckUserAccess();
            
            if (actionResult != null)
            {
                return actionResult;
            }
            
            ValidationResult result = _userValidator.Validate(user);

            if (!result.IsValid)
            {
                return View("~/Views/Error400.cshtml");
            }
                      
            List<User> users = GetUsers();

            if (_validator.UserIsUnique(user ,users))
            {
                User oldUser = users.FirstOrDefault(u => u.Id == user.Id);

                _userRepository.Modify(user);
                users.Remove(oldUser);
                users.Add(user);
                HttpContext.Session.Set(Users_, users);    
                HttpContext.Session.Set("userModifyError", "");
            }
            else
            {
                string modifyError = UserExists;
                HttpContext.Session.Set("userModifyError", modifyError);
            }
            
            return View("UsersActionResult", "Данные успешно обновлены");
        }

        [HttpGet]
        public IActionResult AddUser()
        {        
            IActionResult result = CheckUserAccess();
            
            if (result != null)
            {
                return result;
            }
            
            HttpContext.Session.Set("userAddError", "");          
            return View();
        }
        
        [HttpPost]
        public IActionResult AddUser(User user)
        {
            IActionResult actionResult = CheckUserAccess();
            
            if (actionResult != null)
            {
                return actionResult;
            }

            ValidationResult result = _userValidator.Validate(user);

            if (!result.IsValid)
            {
                return View("~/Views/Error400.cshtml");
            }
            
            List<User> users = GetUsers();

            if (_validator.UserIsUnique(user ,users))
            {
                _userRepository.Add(user);
                users.Add(user);
                HttpContext.Session.Set(Users_, users);
                HttpContext.Session.Set("userAddError", "");                  
            }
            else
            {
                string userAddError = UserExists;
                HttpContext.Session.Set("userAddError", userAddError);
                return View();
            }

            return View("UsersActionResult", "Данные успешно добавлены");
        }       
        
        private List<User> GetUsers()
        {
            List<User> users = HttpContext.Session.Get<List<User>>(Users_) ?? _userRepository.GetAll();

            return users;
        }

        public IActionResult CheckUserAccess()
        {
            User currentUser = HttpContext.Session.Get<User>("user");

            if (currentUser.Role != "admin")
            {
                return View("AccessError");
            }

            return null;
        }
    }
}