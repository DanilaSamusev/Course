namespace AccountingSystem.Models
{
    public class LoginModel
    {
        public string Login { get; set; }
        public int Password { get; set; }
        public string LoginError { get; set; }
        public string PasswordError { get; set; }
        public string AuthenticationError { get; set; }

        public bool IsValid()
        {
            if (Login == null)
            {
                LoginError = "Логин не введён";
                return false;
            }

            if (Password == 0)
            {
                PasswordError = "Пароль не введён";  
                return false;
            }

            return true;
        }
        
    }
}