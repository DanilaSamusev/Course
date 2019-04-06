using System.Collections.Generic;

namespace AccountingSystem.Models
{
    public class ScoresRating
    {
        public long StudentId { get; set; }       
        
        public Dictionary<string, string> rating { get; set; }
    }
}