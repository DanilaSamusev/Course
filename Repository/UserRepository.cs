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

        public User GetOne(string login, int password)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                string strQuery = "Select * from users" +
                                  " where login = @login and password = @password";

                User user = connection.Query<User>(strQuery, new {login, password}).FirstOrDefault();

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
                string strQuery = "Delete from users" +
                                  " where id = @userId";

                connection.Query(strQuery, new {userId});                
            }
        }

        public void Modify(User user)
        {                        
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                string strQuery = "Update users set" +
                                  " login = @Login," +
                                  " password = @Password," +
                                  " role = @Role" +
                                  " where id = @Id";

                connection.Query(strQuery, user);
            }
        }

        public void Add(User user)
        {                      
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                string strQuery = "Insert into users" +
                                  " (login, password, role)" +
                                  " values" +
                                  " (@Login, @Password, @Role)";

                connection.Query(strQuery, user);
            }
        }
        
    }
}