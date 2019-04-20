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
        private IUserRepository _userRepository { get; set; }
        private Validator _validator { get; set; }
        private const string USERS = "users";
        private const string USER_EXISTS = "Пользователь с таким логином уже существует!";

        public UsersController(IUserRepository userRepository, Validator validator)
        {
            _userRepository = userRepository;
            _validator = validator;
        }

        public IActionResult Users()
        {
            IActionResult result = CheckUserAccess();
            
            if (result != null)
            {
                return result;
            }
                                   
            List<User> users = GetUsersFromSession();
                     
            return View(users);
        }

        public IActionResult DeleteUser(long id)
        {            
            IActionResult result = CheckUserAccess();
            
            if (result != null)
            {
                return result;
            }
            
            List<User> users = GetUsersFromSession();

            _userRepository.DeleteOneById(id);
            User user = users.FirstOrDefault(u => u.Id == id);
            users.Remove(user);
            HttpContext.Session.Set(USERS, users);

            return RedirectToAction("UsersResult", "Users", new {message = "Пользователь успешно удалён"});
        }

        public IActionResult ModifyUser(User user)
        {
            IActionResult result = CheckUserAccess();
            
            if (result != null)
            {
                return result;
            }

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
            IActionResult result = CheckUserAccess();
            
            if (result != null)
            {
                return result;
            }

            
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