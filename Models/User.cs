namespace AccountingSystem.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Login { get; set; }
        public int Password { get; set; }
        public string Role { get; set; }
    }
}