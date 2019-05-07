using System.Collections.Generic;
using AccountingSystem.Models;

namespace AccountingSystem.Services
{
    public class StudentComparerByDebts : IComparer<Student>
    {
        public int Compare(Student student1, Student student2)
        {
            return student1.Debts.CompareTo(student2.Debts);
        }
    }
}