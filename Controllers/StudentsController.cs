using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AccountingSystem.Models;
using AccountingSystem.Extensions;
using AccountingSystem.Repository;
using System.Linq;

namespace AccountingSystem.Controllers
{
    public class StudentsController : Controller
    {       
        // MySQL repositories
        private StudentRepository _studentRepository { get; set; }
        private RatingRepository _ratingRepository { get; set; }
        
        public StudentsController(StudentRepository studentRepository, RatingRepository ratingRepository)
        {
            _studentRepository = studentRepository;
            _ratingRepository = ratingRepository;
        }
        
        // This method returns sorted list of students 
        public IActionResult Students()
        {
            List<Student> students = HttpContext.Session.Get<List<Student>>("students");

            if (students == null)
            {
                students = _studentRepository.GetAll();              
            }
            
            FillRating(students);
            students.Sort();
            
            return View(students);
        }

        // This method returns student rating 
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

        // This method fills student rating for displaying 
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

        public IActionResult DeleteStudent(long id)
        {
            List<Student> students = HttpContext.Session.Get<List<Student>>("students");

            if (students == null)
            {
                students = _studentRepository.GetAll();
            }

            Student student = students.FirstOrDefault(s => s.Id == id);
            
            _studentRepository.Delete(student.Id);
            students.Remove(student);
            HttpContext.Session.Set("students", students);
            
            return RedirectToAction("Students", "Students");
        }

        public IActionResult ModifyStudent(Student student)
        {            
            List<Student> students = HttpContext.Session.Get<List<Student>>("students");

            if (students == null)
            {
                students = _studentRepository.GetAll();
            }

            Student oldStudent = students.FirstOrDefault(s => s.Id == student.Id);
            
            _studentRepository.Modify(student);
            students.Remove(oldStudent);
            students.Add(student);
            HttpContext.Session.Set("students", students);
            
            return RedirectToAction("Students", "Students");
        }

        public IActionResult AddStudent(Student student, ExamsRating examsRating, ScoresRating scoresRating)
        {
            student = _studentRepository.Add(student);
            examsRating.StudentId = student.Id;
            scoresRating.StudentId = student.Id;
            _ratingRepository.AddExamRating(examsRating);
            _ratingRepository.AddScoreRating(scoresRating);
            
            return RedirectToAction("Students", "Students");
        }
    }
}