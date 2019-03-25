using System.Collections.Generic;
using System.Linq;
using AccountingSystem.Models;
using Dapper;
using MySql.Data.MySqlClient;

namespace AccountingSystem.Repository
{
    public class StudentRepository
    {        
        private string ConnectionString { get; set; }
        
        public StudentRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public List<Student> GetAll()
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                string strQuery = "Select * from students";

                List<Student> students = connection.Query<Student>(strQuery).ToList();

                return students;
            }
        }

        public void Delete(long studentId)
        {          
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                string strQuery = "Delete from students where id = @studentId";

                connection.Query(strQuery, new {studentId});               
            }
        }
    }
}