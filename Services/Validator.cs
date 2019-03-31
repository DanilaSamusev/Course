using System.Collections.Generic;
using System.Linq;
using AccountingSystem.Models;

namespace AccountingSystem.Services
{
    public class Validator
    {
        private const string SUCCESS = "зачёт";
        private const string FAIL = "незачёт";
        
        public bool UserIsUnique(User user, List<User> users)
        {
            User dublicate = users.FirstOrDefault(u => u.Login == user.Login);

            if (dublicate == null) return true;

            return false;

        }

        public bool ExamsRatingIsValid(ExamsRating examsRating)
        {
            if (examsRating.Mathematics > 10 || examsRating.Mathematics < 0 ||
                examsRating.Philosophy > 10 || examsRating.Philosophy < 0 ||
                examsRating.Physics > 10 || examsRating.Physics < 0 ||
                examsRating.Programming > 10 || examsRating.Programming < 0 ||
                examsRating.Psychology > 10 || examsRating.Psychology < 0)
            {
                return false;
            }

            return true;
        }

        public bool ScoresRatingIsValid(ScoresRating scoresRating)
        {
            if (scoresRating.Chemistry != SUCCESS || scoresRating.Chemistry != FAIL ||
                scoresRating.History != SUCCESS || scoresRating.History != FAIL ||
                scoresRating.ForeignLanguage != SUCCESS || scoresRating.ForeignLanguage != FAIL ||
                scoresRating.PE != SUCCESS || scoresRating.PE != FAIL ||
                scoresRating.PoliticalScience != SUCCESS || scoresRating.PoliticalScience != FAIL)
            {
                return false;
            }

            return true;
        }
    }
}