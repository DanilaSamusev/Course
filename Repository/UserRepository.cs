using System.Collections.Generic;
using System.Linq;
using AccountingSystem.Models;
using Dapper;
using MySql.Data.MySqlClient;

namespace AccountingSystem.Repository
{
    public class UserRepository : IUserRepository
    {

        private string ConnectionString { get; set; }
        
        public UserRepository(string connectionString)
        {
            ConnectionString = connectionString;
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
     
        public User GetOneByLoginAndPassword(LoginModel model)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                string strQuery = "Select * from users" +
                                  " where login = @Login and password = @Password";

                User user = connection.Query<User>(strQuery, model).FirstOrDefault();

                return user;
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
        
        public void DeleteOneById(long userId)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                string strQuery = "Delete from users" +
                                  " where id = @userId";

                connection.Query(strQuery, new {userId});                
            }
        }
        
    }
}