using AccountingSystem.Models;

namespace AccountingSystem.Services
{
    public class ExamsRatingValidator
    {
        public bool IsValid(ExamsRating examsRating)
        {
            foreach (var exam in examsRating.rating)
            {
                if (!IsValid(exam.Value))
                {
                    return false;
                }
            }
            
            return true;                  
        }

        private bool IsValid(int exam)
        {
            if (exam <= 10 && exam >= 0)
            {
                return true;
            }

            return false;
        }
    }
}