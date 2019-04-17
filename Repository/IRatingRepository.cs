using System.Collections.Generic;
using AccountingSystem.Models;

namespace AccountingSystem.Repository
{
    public interface IRatingRepository
    {
        IDictionary<string, object> GetExamsRating(long studentId);

        IDictionary<string, object> GetScoresRating(long studentId);

        void AddExamRating(ExamsRating examsRating);

        void AddScoreRating(ScoresRating scoresRating);

        void Modify(ExamsRating examsRating, ScoresRating scoresRating);                        
    }
}