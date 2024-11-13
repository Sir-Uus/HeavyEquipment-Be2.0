namespace Domain.Entities
{
    public class PerformanceFeedback
    {
        public int Id { get; set; }
        public int EquipmentId { get; set; }
        public Equipment Equipment { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public DateTime FeedbackDate { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public bool IsDeleted { get; set; }
    }
}
