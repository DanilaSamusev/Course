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
        
    }
}