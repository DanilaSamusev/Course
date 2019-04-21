using System.Collections.Generic;
using AccountingSystem.Models;

namespace AccountingSystem.Repository
{
    public interface IUserRepository
    {               
        List<User> GetAll();

        User GetOneByLoginAndPassword(LoginModel model);        

        void Modify(User user);

        void Add(User user);
        
        void DeleteOneById(long userId);
    }
}