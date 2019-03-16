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

        public UsersController(UserRepository userRepository, Validator validator)
        {
            _userRepository = userRepository;
            _validator = validator;
        }

        public IActionResult Users()
        {
            List<User> users = HttpContext.Session.Get<List<User>>("users");

            if (users == null)
            {
                users = _userRepository.GetAll();
            }

            return View(users);
        }

        public IActionResult DeleteUser(long id)
        {
            List<User> users = HttpContext.Session.Get<List<User>>("users");

            if (users == null)
            {
                users = _userRepository.GetAll();
            }

            _userRepository.Delete(id);
            User user = users.FirstOrDefault(u => u.Id == id);
            users.Remove(user);
            HttpContext.Session.Set("users", users);

            return RedirectToAction("Users", "Users");
        }

        public IActionResult ModifyUser(User user)
        {
            List<User> users = HttpContext.Session.Get<List<User>>("users");

            if (users == null)
            {
                users = _userRepository.GetAll();
            }

            User oldUser = users.FirstOrDefault(u => u.Id == user.Id);

            _userRepository.Modify(user);
            users.Remove(oldUser);
            users.Add(user);
            HttpContext.Session.Set("users", users);

            return RedirectToAction("Users", "Users");
        }

        public IActionResult AddUser(User user)
        {
            List<User> users = HttpContext.Session.Get<List<User>>("users");

            if (users == null)
            {
                users = _userRepository.GetAll();
            }

            if (_validator.IsUnique(user ,users))
            {
                _userRepository.Add(user);
                users.Add(user);
                HttpContext.Session.Set("users", users);              
            }
            else
            {
                string error = "Пользователь с таким логином уже существует";
                HttpContext.Session.Set("resultError", error);
            }
            
            return RedirectToAction("Users", "Users");
        }
    }
}