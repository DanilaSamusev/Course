namespace AccountingSystem.Models
{
    public class ExamsRating
    {
        public long StudentId { get; set; }
        public int Philosophy { get; set; }
        public int Psychology { get; set; }
        public int Mathematics { get; set; }
        public int Physics { get; set; }
        public int Programming { get; set; }
    }
}