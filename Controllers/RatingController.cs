using System.Collections.Generic;
using System.Linq;
using AccountingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using AccountingSystem.Extensions;
using AccountingSystem.Repository;
using FluentValidation;
using FluentValidation.Results;

namespace AccountingSystem.Controllers
{
    public class RatingController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IRatingRepository _ratingRepository;
        private readonly AbstractValidator<ExamsRating> _examsRatingValidator;
        private readonly AbstractValidator<ScoresRating> _scoresRatingValidator;
        private const string Students_ = "students";
        
        public RatingController(IRatingRepository ratingRepository, IStudentRepository studentRepository,
            AbstractValidator<ExamsRating> examsValidator, AbstractValidator<ScoresRating> scoresRatingValidator)
        {
            _ratingRepository = ratingRepository;
            _studentRepository = studentRepository;
            _examsRatingValidator = examsValidator;
            _scoresRatingValidator = scoresRatingValidator;
        }
        
        public IActionResult Rating(long studentId)
        {
            if (studentId < 0)
            {
                return View("~/Views/Error400.cshtml");
            }
            
            List<Student> students = GetStudents();
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
            ValidationResult examValidationResult = _examsRatingValidator.Validate(examsRating);
            ValidationResult scoreValidationResult = _scoresRatingValidator.Validate(scoresRating);

            if (!examValidationResult.IsValid || !scoreValidationResult.IsValid)
            {
                return View("~/Views/Error400.cshtml");
            }
            
            _ratingRepository.Modify(examsRating, scoresRating);

            return View("Result");          
        }
        
        private List<Student> GetStudents()
        {
            List<Student> students = HttpContext.Session.Get<List<Student>>(Students_) ?? _studentRepository.GetAll();

            return students;
        }
    }
    
    
}