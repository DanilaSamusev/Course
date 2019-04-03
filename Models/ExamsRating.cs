using System.Collections.Generic;

namespace AccountingSystem.Models
{
    public class ExamsRating
    {
        public long StudentId { get; set; }                       
        public Dictionary<string, int> rating { get; set; }
    }
}