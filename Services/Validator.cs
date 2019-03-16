using System.Collections.Generic;
using System.Linq;
using AccountingSystem.Models;

namespace AccountingSystem.Services
{
    public class Validator
    {
        public bool IsUnique(User user, List<User> users)
        {
            User dublicate = users.FirstOrDefault(u => u.Login == user.Login);

            if (dublicate == null) return true;

            return false;

        }
    }
}