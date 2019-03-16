using System.Collections.Generic;
using System.Linq;
using AccountingSystem.Models;
using Dapper;
using MySql.Data.MySqlClient;

namespace AccountingSystem.Repository
{
    public class UserRepository
    {

        private string ConnectionString { get; set; }
        
        public UserRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public User GetOne(string login)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                string strQuery = "Select * from users where login = @login";

                User user = connection.Query<User>(strQuery, new {login}).FirstOrDefault();

                return user;
            }            
        }

        public List<User> GetAll()
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                string strQuery = "Select * from users";

                List<User> users = connection.Query<User>(strQuery).ToList();

                return users;
            }
        }

        public void Delete(long userId)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                string strQuery = "Delete from users where id = @userId";

                connection.Query(strQuery, new {userId});                
            }
        }

        public void Modify(User user)
        {
            long id = user.Id;
            string login = user.Login;
            int password = user.Password;
            string role = user.Role;
            
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                string strQuery = "Update users set login = @login, password = @password, role = @role where id = @id";

                connection.Query(strQuery, new {id, login, password, role});
            }
        }

        public void Add(User user)
        {           
            string login = user.Login;
            int password = user.Password;
            string role = user.Role;

            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                string strQuery = "Insert into users (login, password, role)" +
                                  "values(@login, @password, @role)";

                connection.Query(strQuery, new {login, password, role});
            }
        }
        
    }
}