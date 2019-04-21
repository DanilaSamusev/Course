using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AccountingSystem.Models;
using AccountingSystem.Extensions;
using AccountingSystem.Repository;
using System.Linq;
using AccountingSystem.Services;
using FluentValidation;
using FluentValidation.Results;

namespace AccountingSystem.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IRatingRepository _ratingRepository;
        private readonly AbstractValidator<ExamsRating> _examsRatingValidator;
        private readonly AbstractValidator<ScoresRating> _scoresRatingValidator;
        private readonly AbstractValidator<Student> _studentValidator;
        private const string Students_ = "students";

        public StudentsController(IStudentRepository studentRepository, IRatingRepository ratingRepository,
            Validator validator,
            AbstractValidator<ExamsRating> examsRatingValidator, AbstractValidator<ScoresRating> scoresRatingValidator,
            AbstractValidator<Student> studentValidator)
        {
            _studentRepository = studentRepository;
            _ratingRepository = ratingRepository;
            _examsRatingValidator = examsRatingValidator;
            _scoresRatingValidator = scoresRatingValidator;
            _studentValidator = studentValidator;
        }

        public IActionResult Students()
        {
            List<Student> students = GetStudents();
            
            //////
            List<Student> requiredStudents = HttpContext.Session.Get<List<Student>>("requiredStudents");
            bool searchIsActive = HttpContext.Session.Get<bool>("searchIsActive");
            //////
            
            if (requiredStudents == null || requiredStudents.Count == 0)
            {
                if (searchIsActive)
                {                   
                    ResetSearch();
                    return RedirectToAction("StudentsResult", "Students",
                        new {message = "По вашему запросу ничего не найдено"});
                }
            }
            else
            {
                students = requiredStudents;               
            }
          
            FillRating(students);
            students.Sort();
            return View(students);
        }

        public IActionResult DeleteStudent(long id)
        {
            if (id < 0)
            {
                return View("~/Views/Error400.cshtml");
            }

            List<Student> students = GetStudents();
            Student student = students.FirstOrDefault(s => s.Id == id);

            _studentRepository.Delete(student);
            students.Remove(student);
            HttpContext.Session.Set(Students_, students);
            ResetSearch();
            
            return RedirectToAction("StudentsResult", "Students",
                new {message = "Студент успешно удалён"});
        }

        public IActionResult UpdateStudent(Student student)
        {
            ValidationResult validationResult = _studentValidator.Validate(student);

            if (!validationResult.IsValid)
            {
                return View("~/Views/Error400.cshtml"); 
            }
            
            List<Student> students = GetStudents();

            Student oldStudent = students.FirstOrDefault(s => s.Id == student.Id);

            _studentRepository.Modify(student);
            students.Remove(oldStudent);
            students.Add(student);
            HttpContext.Session.Set(Students_, students);

            ResetSearch();
            
            return RedirectToAction("StudentsResult", "Students",
                new {message = "Студент успешно обновлен"});
        }

        [HttpGet]
        public IActionResult AddStudent()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddStudent(Student student, ExamsRating examsRating, ScoresRating scoresRating)
        {
            List<Student> students = GetStudents();

            ValidationResult examValidationResult = _examsRatingValidator.Validate(examsRating);
            ValidationResult scoreValidationResult = _scoresRatingValidator.Validate(scoresRating);
            ValidationResult studetnValidationResult = _studentValidator.Validate(student);

            if (!examValidationResult.IsValid || !scoreValidationResult.IsValid || !studetnValidationResult.IsValid)
            {
                return View("~/Views/Error400.cshtml");
            }

            student = _studentRepository.Add(student);
            examsRating.StudentId = student.Id;
            scoresRating.StudentId = student.Id;
            _ratingRepository.AddExamRating(examsRating);
            _ratingRepository.AddScoreRating(scoresRating);           
            students.Add(student);
            HttpContext.Session.Set(Students_, students);

            ResetSearch();
            
            return RedirectToAction("StudentsResult", "Students",
                new {message = "Студент успешно добавлен"});
        }

        public IActionResult StudentsResult(string message)
        {
            return View(model: message);
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
            List<Student> students = GetStudents();

            switch (option)
            {
                case "sortByDebts":
                {
                    int debts;

                    try
                    {
                        debts = int.Parse(value);
                    }
                    catch
                    {                        
                        ResetSearch();
                        return RedirectToAction("StudentsResult", "Students",
                            new {message = "Некорректные данные"});
                    }

                    FillRating(students);
                    requiredStudents = students.Where(s => s.Debts == debts).ToList();

                    return SetSearch(requiredStudents);
                }
                case "sortByGroup":
                {
                    int groupNumber;

                    try
                    {
                        groupNumber = int.Parse(value);
                    }
                    catch
                    {
                        ResetSearch();
                        return RedirectToAction("StudentsResult", "Students",
                            new {message = "Некорректные данные"});
                    }

                    requiredStudents = students.Where(s => s.Group_Number == groupNumber).ToList();

                    return SetSearch(requiredStudents);
                }
                case "sortBySurname":
                {
                    string surname = value;
                    requiredStudents = students.Where(s => s.Surname == surname).ToList();

                    return SetSearch(requiredStudents);
                }
            }

            return RedirectToAction("Students", "Students");
        }

        private IActionResult SetSearch(List<Student> requiredStudents)
        {
            HttpContext.Session.Set("requiredStudents", requiredStudents);
            HttpContext.Session.Set("searchIsActive", true);
            return RedirectToAction("Students", "Students");
        }

        private void ResetSearch()
        {
            HttpContext.Session.Set("requiredStudents", new List<Student>());
            HttpContext.Session.Set("searchIsActive", false);            
        }        

        private List<Student> GetStudents()
        {
            List<Student> students = HttpContext.Session.Get<List<Student>>(Students_) ?? _studentRepository.GetAll();

            return students;
        }
    }
}