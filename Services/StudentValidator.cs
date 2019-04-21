using AccountingSystem.Models;
using FluentValidation;

namespace AccountingSystem.Services
{
    public class StudentValidator : AbstractValidator<Student>
    {
        public StudentValidator()
        {
            RuleFor(student => student.Name).NotNull();
            RuleFor(student => student.Surname).NotNull();
            RuleFor(student => student.Patronymic).NotNull();
            RuleFor(student => student.Group_Number.ToString().Length > 6);
        }
    }
}