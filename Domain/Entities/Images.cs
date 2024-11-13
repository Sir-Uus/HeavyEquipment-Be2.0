namespace Domain.Entities
{
    public class Images
    {
        public int Id { get; set; }
        public int EquipmentId { get; set; }
        public string FileName { get; set; }
        public string ContenType { get; set; }
        public string Image { get; set; }
        public bool IsDeleted { get; set; }
        public Equipment Equipment { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}