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

        public IActionResult DeleteUser(long userId)
        {
            List<User> users = HttpContext.Session.Get<List<User>>("users");
            
            if (users == null)
            {
                users = _userRepository.GetAll();
            }
            
            User user = users.FirstOrDefault(u => u.Id == userId);
            users.Remove(user);
            _userRepository.Delete(userId);
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

            users.Remove(oldUser);
            users.Add(user);
            _userRepository.Modify(user);
            HttpContext.Session.Set("users", users);
            
            return RedirectToAction("Users", "Users");
        }
    }
}