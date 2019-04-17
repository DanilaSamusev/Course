using System.Collections.Generic;
using AccountingSystem.Models;

namespace AccountingSystem.Repository
{
    public interface IUserRepository
    {               
        List<User> GetAll();

        User GetOneByLoginAndPassword(string login, int password);        

        void Modify(User user);

        void Add(User user);
        
        void DeleteOneById(long userId);
    }
}