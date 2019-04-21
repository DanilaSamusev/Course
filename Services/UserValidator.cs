using AccountingSystem.Models;
using FluentValidation;

namespace AccountingSystem.Services
{
    public class UserValidator : AbstractValidator<User>
    {

        public UserValidator()
        {
            RuleFor(user => user.Id).GreaterThanOrEqualTo(0);
            RuleFor(user => user.Login).NotNull();
            RuleFor(user => user.Password).NotEqual(0);
            RuleFor(user => user.Role).NotNull();
        }
        
    }
}