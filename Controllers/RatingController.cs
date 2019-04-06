using System.Collections.Generic;
using System.Linq;
using AccountingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using AccountingSystem.Extensions;
using AccountingSystem.Repository;
using AccountingSystem.Services;

namespace AccountingSystem.Controllers
{
    public class RatingController : Controller
    {
        private StudentRepository _studentRepository { get; set; }
        private RatingRepository _ratingRepository { get; set; }
        private ExamsRatingValidator _examsRatingValidator { get; set; }
        private ScoresRatingValidator _scoresRatingValidator { get; set; }
        private const string STUDENTS = "students";
        
        public RatingController(RatingRepository ratingRepository, StudentRepository studentRepository,
            ExamsRatingValidator examsValidator, ScoresRatingValidator scoresRatingValidator)
        {
            _ratingRepository = ratingRepository;
            _studentRepository = studentRepository;
            _examsRatingValidator = examsValidator;
            _scoresRatingValidator = scoresRatingValidator;
        }
        
        public IActionResult Rating(long studentId)
        {                      
            List<Student> students = GetStudentsFromSession();
            Student currentStudent = students.FirstOrDefault(s => s.Id == studentId);
            
            HttpContext.Session.Set("currentStudent", currentStudent);
            IDictionary<string, object> examsRating = _ratingRepository.GetExamsRating(currentStudent.Id);
            IDictionary<string, object> scoresRating = _ratingRepository.GetScoresRating(currentStudent.Id);           
            HttpContext.Session.Set("examsRating", examsRating);
            HttpContext.Session.Set("scoresRating", scoresRating);
            
            return View();
        }

        [HttpGet]
        public IActionResult ModifyRating(long studentId)
        {
            HttpContext.Session.Set("currentStudentId", studentId);
            return View();
        }
        
        [HttpPost]
        public IActionResult ModifyRating(ExamsRating examsRating, ScoresRating scoresRating)
        {
            if (!_examsRatingValidator.IsValid(examsRating))
            {
                HttpContext.Session.Set("error", "Ошибка! Отметка по экзамену должна быть числом, больше 1 и меньше 11");
                return View("RatingError");
            }

            if (!_scoresRatingValidator.IsValid(scoresRating))
            {
                HttpContext.Session.Set("error", "Ошибка! В поля зачётов должны вписываться \"зачёт\" или \"незачёт\"");
                return View("RatingError");
            }
            
            _ratingRepository.Modify(examsRating, scoresRating);

            return View("Result");          
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