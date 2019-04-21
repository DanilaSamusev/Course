using System.Collections.Generic;
using System.Linq;
using AccountingSystem.Models;

namespace AccountingSystem.Services
{
    public class Validator
    {             
        public bool UserIsUnique(User user, List<User> users)
        {
            User duplicate = users.FirstOrDefault(u => u.Login == user.Login);

            return duplicate == null;
        }

        
    }
}