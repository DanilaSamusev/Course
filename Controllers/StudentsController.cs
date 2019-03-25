using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AccountingSystem.Models;
using AccountingSystem.Extensions;
using AccountingSystem.Repository;
using System.Linq;
using System.Text.RegularExpressions;

namespace AccountingSystem.Controllers
{
    public class StudentsController : Controller
    {       
        private StudentRepository _studentRepository { get; set; }
        private RatingRepository _ratingRepository { get; set; }
        
        public StudentsController(StudentRepository studentRepository, RatingRepository ratingRepository)
        {
            _studentRepository = studentRepository;
            _ratingRepository = ratingRepository;
        }
        
        public IActionResult Students()
        {
            List<Student> students = HttpContext.Session.Get<List<Student>>("students");

            if (students == null)
            {
                students = _studentRepository.GetAll();
                FillRating(students);
            }
            
            students.Sort();
            
            return View(students);
        }

        public IActionResult Rating(long studentId)
        {
            List<Student> students = HttpContext.Session.Get<List<Student>>("students");

            if (students == null)
            {
                students = _studentRepository.GetAll();
            }

            Student currentStudent = students.FirstOrDefault(s => s.Id == studentId);
            HttpContext.Session.Set("currentStudent", currentStudent);

            IDictionary<string, object> examsRating = _ratingRepository.GetExamsRating(currentStudent.Id);
            IDictionary<string, object> scoresRating = _ratingRepository.GetScoresRating(currentStudent.Id);           
            HttpContext.Session.Set("examsRating", examsRating);
            HttpContext.Session.Set("scoresRating", scoresRating);
            
            return View();
        }


        private void FillRating(List<Student> students)
        {
            foreach (Student student in students)    
            {                              
                int debts = 0;

                IDictionary<string, object> examsRating = _ratingRepository.GetExamsRating(student.Id);
                IDictionary<string, object> scoresRating = _ratingRepository.GetScoresRating(student.Id);

                int examsDebts = examsRating.Values.Count(ex => (int) ex < 4);
                int scoresDebts = scoresRating.Values.Count(sc => sc.Equals("незачёт"));

                debts += examsDebts + scoresDebts;
                student.Debts = debts;
            }
        }
    }
}