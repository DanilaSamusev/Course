namespace AccountingSystem.Models
{
    public class LoginModel
    {
        public string Login { get; set; }
        public int Password { get; set; }
        public string LoginError { get; set; }
        public string PasswordError { get; set; }
    }
}