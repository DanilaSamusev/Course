using AccountingSystem.Models;
using FluentValidation;

namespace AccountingSystem.Services
{
    public class ScoresRatingValidator : AbstractValidator<ScoresRating>
    {
        public ScoresRatingValidator()
        {
            RuleForEach(scores => scores.rating).Where(rating => rating.Value == "зачёт" || rating.Value == "незачёт");
        }                
    }
}