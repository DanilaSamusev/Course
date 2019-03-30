using System;

namespace AccountingSystem.Models
{
    public class Student : IComparable<Student>
    {        
        public long Id { get; set; }
        public int GroupNumber { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public int Debts { get; set; }


        public int CompareTo(Student student)
        {
            if (student == null)
            {
                return 1;
            }
            else
            {
                return this.Debts.CompareTo(student.Debts);
            }            
        }
    }
}