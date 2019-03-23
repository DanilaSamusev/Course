namespace AccountingSystem.Models
{
    public class Student
    {        
        public long Id { get; set; }
        public int GroupNumber { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public int Debts { get; set; }
    }
}