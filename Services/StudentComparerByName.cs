using System.Collections.Generic;
using AccountingSystem.Models;

namespace AccountingSystem.Services
{
    public class StudentComparerByName : IComparer<Student>
    {
        public int Compare(Student student1, Student student2)
        {            
            return student1.Name.CompareTo(student2.Name);            
        }
    }
}