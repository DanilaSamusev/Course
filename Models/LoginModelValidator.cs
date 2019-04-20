using FluentValidation;

namespace AccountingSystem.Models
{
    public class LoginModelValidator : AbstractValidator<LoginModel>
    {
        public LoginModelValidator()
        {
            RuleFor(loginModel => loginModel.Login).NotNull();
            RuleFor(loginModel => loginModel.Password).NotNull();
        }
    }
}