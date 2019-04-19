using System.Collections.Generic;
using System.Linq;
using AccountingSystem.Models;
using Dapper;
using MySql.Data.MySqlClient;

namespace AccountingSystem.Repository
{
    public class StudentRepository : IStudentRepository
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
                string strQuery = "Delete from students" +
                                  " where id = @studentId";

                connection.Query(strQuery, new {studentId});               
            }
        }

        public void Modify(Student student)
        {           
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                string strQuery =
                    "Update students set" +
                    " group_number = @GroupNumber," +
                    " name = @Name," +
                    " surname = @Surname," +
                    " patronymic = @Patronymic" +
                    " where id = @Id";

                connection.Query(strQuery, student);
            }
        }

        public Student Add(Student student)
        {                                   
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                string strQuery = "Insert into students" +
                                  " (group_number, name, surname, patronymic) values" +
                                  " (@GroupNumber, @Name, @Surname, @Patronymic);" +
                                  " Select LAST_INSERT_ID();";
                                             
                long id = connection.Query<long>(strQuery, student).FirstOrDefault();
                student.Id = id;

                return student;
            }
        }
    }
}