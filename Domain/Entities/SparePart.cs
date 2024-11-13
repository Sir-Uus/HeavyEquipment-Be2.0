namespace Domain.Entities
{
    public class SparePart
    {
        public int Id { get; set; }
        public int EquipmentId { get; set; }
        public string PartName { get; set; }
        public string PartNumber { get; set; }
        public string Manufacturer { get; set; }
        public string AvailabilityStatus { get; set; }
        public decimal Price { get; set; }
        public bool IsDeleted { get; set; }
        public int? Stock { get; set; }
        public SparePartImage SparePartImage { get; set; }
        public Equipment Equipment { get; set; }
        public ICollection<SparePartFeedback> SparePartFeedbacks { get; set; }
        public ICollection<TransactionDetail> TransactionDetails { get; set; }
    }
}
