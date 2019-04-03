using AccountingSystem.Models;

namespace AccountingSystem.Services
{
    public class ScoresRatingValidator
    {        
        private const string SUCCESS = "зачёт";
        private const string FAIL = "незачёт";
        
        public bool ScoresRatingIsValid(ScoresRating scoresRating)
        {
            foreach (var pair in scoresRating.rating)
            {
                if (!IsValid(pair.Value))
                {
                    return false;
                }
            }

            return true;
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