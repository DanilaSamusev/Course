using System.Collections.Generic;
using AccountingSystem.Models;

namespace AccountingSystem.Repository
{
    public interface IStudentRepository
    {
        List<Student> GetAll();

        void Delete(long studentId);

        void Modify(Student student);

        Student Add(Student student);
    }
}