using System.Collections.Generic;
using AccountingSystem.Models;

namespace AccountingSystem.Services
{
    public class StudentComparerByGroupNumber : IComparer<Student>
    {
        public int Compare(Student student1, Student student2)
        {            
            return student1.Group_Number.CompareTo(student2.Group_Number);            
        }
    }
}