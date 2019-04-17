using System.Collections.Generic;
using System.Linq;
using AccountingSystem.Repository;
using Microsoft.AspNetCore.Mvc;
using AccountingSystem.Extensions;
using AccountingSystem.Models;
using AccountingSystem.Services;

namespace AccountingSystem.Controllers
{
    public class UsersController : Controller
    {
        private UserRepository _userRepository { get; set; }
        private Validator _validator { get; set; }
        private const string USERS = "users";
        private const string USER_EXISTS = "Пользователь с таким логином уже существует!";

        public UsersController(UserRepository userRepository, Validator validator)
        {
            _userRepository = userRepository;
            _validator = validator;
        }

        public IActionResult Users()
        {
            User user = HttpContext.Session.Get<User>("user");

            if (user.Role != "admin")
            {
                return View("AccessError");
            }
            
            List<User> users = GetUsersFromSession();
                     
            return View(users);
        }

        public IActionResult DeleteUser(long id)
        {
            List<User> users = GetUsersFromSession();

            _userRepository.Delete(id);
            User user = users.FirstOrDefault(u => u.Id == id);
            users.Remove(user);
            HttpContext.Session.Set(USERS, users);

            return RedirectToAction("UsersResult", "Users", new {message = "Пользователь успешно удалён"});
        }

        public IActionResult ModifyUser(User user)
        {
            List<User> users = GetUsersFromSession();

            if (_validator.UserIsUnique(user ,users))
            {
                User oldUser = users.FirstOrDefault(u => u.Id == user.Id);

                _userRepository.Modify(user);
                users.Remove(oldUser);
                users.Add(user);
                HttpContext.Session.Set(USERS, users);    
                HttpContext.Session.Set("userModifyError", "");
            }
            else
            {
                string modifyError = USER_EXISTS;
                HttpContext.Session.Set("userModifyError", modifyError);
            }
            
            return RedirectToAction("UsersResult", "Users", new {message = "Пользователь успешно обновлён"});
        }

        [HttpGet]
        public IActionResult AddUser()
        {        
            HttpContext.Session.Set("userAddError", "");          
            return View();
        }
        
        [HttpPost]
        public IActionResult AddUser(User user)
        {
            List<User> users = GetUsersFromSession();

            if (_validator.UserIsUnique(user ,users))
            {
                _userRepository.Add(user);
                users.Add(user);
                HttpContext.Session.Set(USERS, users);
                HttpContext.Session.Set("userAddError", "");                  
            }
            else
            {
                string userAddError = USER_EXISTS;
                HttpContext.Session.Set("userAddError", userAddError);
                return View();
            }

            return RedirectToAction("UsersResult", "Users", new {message = "Пользователь успешно добавлен"});
        }

        public IActionResult UsersResult(string message)
        {
            return View(model: message);
        }
        
        private List<User> GetUsersFromSession()
        {
            List<User> users = HttpContext.Session.Get<List<User>>(USERS);

            if (users == null)
            {
                users = _userRepository.GetAll();
            }

            return users;
        }
    }
}