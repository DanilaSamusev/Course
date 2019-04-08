using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AccountingSystem.Models;
using AccountingSystem.Extensions;
using AccountingSystem.Repository;
using System.Linq;
using AccountingSystem.Services;

namespace AccountingSystem.Controllers
{
    public class StudentsController : Controller
    {
        private StudentRepository _studentRepository { get; set; }
        private RatingRepository _ratingRepository { get; set; }
        private ExamsRatingValidator _examsRatingValidator { get; set; }
        private ScoresRatingValidator _scoresRatingValidator { get; set; }
        private StudentValidator _studentValidator { get; set; }
        private const string STUDENTS = "students";

        public StudentsController(StudentRepository studentRepository, RatingRepository ratingRepository,
            Validator validator,
            ExamsRatingValidator examsRatingValidator, ScoresRatingValidator scoresRatingValidator,
            StudentValidator studentValidator)
        {
            _studentRepository = studentRepository;
            _ratingRepository = ratingRepository;
            _examsRatingValidator = examsRatingValidator;
            _scoresRatingValidator = scoresRatingValidator;
            _studentValidator = studentValidator;
        }
       
        public IActionResult Students()
        {
            List<Student> students = GetStudentsFromSession();
            List<Student> requiredStudents = HttpContext.Session.Get<List<Student>>("requiredStudents"); 
            
            bool searchIsActive = HttpContext.Session.Get<bool>("searchIsActive");

            if (requiredStudents == null || requiredStudents.Count == 0)
            {
                if (searchIsActive)
                {
                    HttpContext.Session.Set("studentsError", "По вашему запросу ничего не найдено");
                    HttpContext.Session.Set("requiredStudents", new List<Student>());
                    HttpContext.Session.Set("searchIsActive", false);
                    return RedirectToAction("StudentsError", "Students");                  
                }               
            }
            else
            {
                FillRating(requiredStudents);
                requiredStudents.Sort();
                return View(requiredStudents);
            }                     
            
            FillRating(students);
            students.Sort();
            return View(students);
        }

        public IActionResult DeleteStudent(long id)
        {
            List<Student> students = GetStudentsFromSession();

            Student student = students.FirstOrDefault(s => s.Id == id);

            _studentRepository.Delete(student.Id);
            students.Remove(student);
            HttpContext.Session.Set(STUDENTS, students);
            HttpContext.Session.Set("requiredStudents", new List<Student>());
            HttpContext.Session.Set("searchIsActive", false);
            
            return RedirectToAction("Students", "Students");
        }

        public IActionResult UpdateStudent(Student student)
        {
            List<Student> students = GetStudentsFromSession();

            Student oldStudent = students.FirstOrDefault(s => s.Id == student.Id);

            _studentRepository.Modify(student);
            students.Remove(oldStudent);
            students.Add(student);
            HttpContext.Session.Set(STUDENTS, students);
            HttpContext.Session.Set("requiredStudents", new List<Student>());
            HttpContext.Session.Set("searchIsActive", false);

            return RedirectToAction("Students", "Students");
        }

        [HttpGet]
        public IActionResult AddStudent()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddStudent(Student student, ExamsRating examsRating, ScoresRating scoresRating)
        {
            if (!_examsRatingValidator.IsValid(examsRating) || !_scoresRatingValidator.IsValid(scoresRating) ||
                !_studentValidator.IsValid(student))
            {
                HttpContext.Session.Set("RatingError",
                    "Некорректый ввод! Обратите внимание, группа должна быть числом из 6 цифр");
                return View();
            }

            student = _studentRepository.Add(student);
            examsRating.StudentId = student.Id;
            scoresRating.StudentId = student.Id;
            _ratingRepository.AddExamRating(examsRating);
            _ratingRepository.AddScoreRating(scoresRating);
            HttpContext.Session.Set("RatingError", "");
            HttpContext.Session.Set("requiredStudents", new List<Student>());
            HttpContext.Session.Set("searchIsActive", false);

            return RedirectToAction("Students", "Students");
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

        public IActionResult Search(string option, string value)
        {            
            List<Student> requiredStudents;
            List<Student> students = GetStudentsFromSession();
            
            switch (option)
            {
                case "sortByDebts":
                {
                    int debts;
                    
                    try
                    {
                        debts = ParseStringToInt(value);
                    }
                    catch
                    {
                        HttpContext.Session.Set("studentsError", "Некорректные данные");
                        HttpContext.Session.Set("requiredStudents", new List<Student>());
                        HttpContext.Session.Set("searchIsActive", false);
                        return RedirectToAction("StudentsError", "Students");
                    }
                    
                    FillRating(students);
                    requiredStudents = students.Where(s => s.Debts == debts).ToList();
                    HttpContext.Session.Set("requiredStudents", requiredStudents);
                    HttpContext.Session.Set("searchIsActive", true);
                    return RedirectToAction("Students", "Students");                    
                }
                case "sortByGroup":
                {
                    int groupNumber;
                    
                    try
                    {
                        groupNumber = ParseStringToInt(value);
                    }
                    catch
                    {
                        HttpContext.Session.Set("studentsError", "Некорректные данные");
                        HttpContext.Session.Set("requiredStudents", new List<Student>());
                        HttpContext.Session.Set("searchIsActive", false);
                        return RedirectToAction("StudentsError", "Students");
                    }
                    requiredStudents = students.Where(s => s.GroupNumber == groupNumber).ToList();
                    HttpContext.Session.Set("requiredStudents", requiredStudents);
                    HttpContext.Session.Set("searchIsActive", true);
                    return RedirectToAction("Students", "Students");       
                }
                case "sortBySurname":
                {
                    string surname = value;
                    requiredStudents = students.Where(s => s.Surname == surname).ToList();
                    HttpContext.Session.Set("requiredStudents", requiredStudents);
                    HttpContext.Session.Set("searchIsActive", true);
                    return RedirectToAction("Students", "Students");      
                }                                                             
            }
            
            return RedirectToAction("Students", "Students");
        }

        public IActionResult ResetSearch()
        {
            HttpContext.Session.Set("requiredStudents", new List<Student>());
            HttpContext.Session.Set("searchIsActive", false);
            return RedirectToAction("Students", "Students");    
        }
        
        private int ParseStringToInt(string str)
        {
            return int.Parse(str);
        }

        public IActionResult StudentsError(string errorMessage)
        {
            return View(errorMessage);
        }
        
        private List<Student> GetStudentsFromSession()
        {
            List<Student> students = HttpContext.Session.Get<List<Student>>(STUDENTS);

            if (students == null)
            {
                students = _studentRepository.GetAll();
            }

            return students;
        }
    }
}