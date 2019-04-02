using AccountingSystem.Models;

namespace AccountingSystem.Services
{
    public class ScoresRatingValidator
    {        
        private const string SUCCESS = "зачёт";
        private const string FAIL = "незачёт";
        
        public bool ScoresRatingIsValid(ScoresRating scoresRating)
        {
            if (IsValid(scoresRating.Chemistry) &&
                IsValid(scoresRating.History) &&
                IsValid(scoresRating.ForeignLanguage) &&
                IsValid(scoresRating.PE) &&
                IsValid(scoresRating.PoliticalScience))
            {
                return true;
            }

            return false;
        }

        private bool IsValid(string score)
        {
            if (score == SUCCESS || score == FAIL)
            {
                return true;
            }

            return false;
        }
    }
}