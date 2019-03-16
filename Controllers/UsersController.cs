using System.Collections.Generic;
using System.Linq;
using AccountingSystem.Repository;
using Microsoft.AspNetCore.Mvc;
using AccountingSystem.Extensions;
using AccountingSystem.Models;

namespace AccountingSystem.Controllers
{
    public class UsersController : Controller
    {        
        private UserRepository _userRepository { get; set; }

        public UsersController(UserRepository userRepository)
        {
            _userRepository = userRepository;
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
            
            _userRepository.Add(user);            
            users.Add(user);
            HttpContext.Session.Set("users", users);
                        
            return RedirectToAction("Users", "Users");
        }
    }
}