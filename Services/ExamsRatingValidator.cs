using AccountingSystem.Models;
using FluentValidation;

namespace AccountingSystem.Services
{
    public class ExamsRatingValidator : AbstractValidator<ExamsRating>
    {
        public ExamsRatingValidator()
        {
            RuleForEach(exams => exams.rating).Where(rating => rating.Value > 0 && rating.Value < 11);
        }                
    }
}