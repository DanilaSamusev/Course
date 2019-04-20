using AccountingSystem.Models;

namespace AccountingSystem.Services
{
    public class StudentValidator
    {
        public bool IsValid(Student student)
        {
            if (IsValid(student.Name) && IsValid(student.Patronymic) && IsValid(student.Surname) &&
                IsValid(student.Group_Number) && GroupNumberIsValid(student.Group_Number.ToString()))
            {
                return true;
            }

            return false;
        }

        private bool IsValid<T>(T field)
        {
            if (field == null)
            {
                return false;
            }

            return true;
        }

        private bool GroupNumberIsValid(string groupNumber)
        {
            if (groupNumber.Length == 6)
            {
                return true;
            }

            return false;
        }
    }
}