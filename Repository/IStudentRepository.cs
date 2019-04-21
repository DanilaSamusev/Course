using System.Collections.Generic;
using AccountingSystem.Models;

namespace AccountingSystem.Repository
{
    public interface IStudentRepository
    {
        List<Student> GetAll();

        void Delete(Student student);

        void Modify(Student student);

        Student Add(Student student);
    }
}