using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using MySql.Data.MySqlClient;
using AccountingSystem.Models;
using Newtonsoft.Json.Linq;

namespace AccountingSystem.Repository
{
    public class RatingRepository
    {
        private string ConnectionString { get; set; }

        public RatingRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public IDictionary<string, object> GetExamsRating(long studentId)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                string strQuery = "Select philosophy, psychology, mathematics, physics, programming from exams where student_id = @studentId";

                IDictionary<string, object> examsRating = connection.Query(strQuery, new {studentId}).SingleOrDefault();                

                return examsRating;
            }
        }
        
        public IDictionary<string, object> GetScoresRating(long studentId)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                string strQuery = "Select history, political_science, PE, foreign_language, chemistry from scores where student_id = @studentId";

                IDictionary<string, object> scoresRating =  connection.Query(strQuery, new {studentId}).SingleOrDefault();               

                return scoresRating;
            }
        }              
    }
}