using System.Collections.Generic;
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
        
    }
}