namespace Domain.Entities
{
    public class RentalRequest
    {
        public int Id { get; set; }
        public string Invoice { get; set; }
        public string UserId { get; set; }
        public int EquipmentId { get; set; }
        public DateTime StarDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; }
        public User User { get; set; }
        public Equipment Equipment { get; set; }
        public ICollection<Payment> Payments { get; set; }

    }
}
