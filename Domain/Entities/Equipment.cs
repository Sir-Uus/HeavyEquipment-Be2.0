namespace Domain.Entities
{
    public class Equipment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string YearOfManufacture { get; set; }
        public string Specification { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Location { get; set; }
        public decimal RentalPrice { get; set; }
        public bool IsDeleted { get; set; }
        public int? Unit { get; set; }
        public Images Images { get; set; }
        public ICollection<PerformanceFeedback> PerformanceFeedbacks { get; set; }
        public ICollection<MaintenancedRecord> MaintenancedRecords { get; set; }
        public ICollection<RentalHistory> RentalHistories { get; set; }
        public ICollection<SparePart> SpareParts { get; set; }
        public ICollection<RentalRequest> RentalRequests { get; set; }
        public ICollection<TransactionDetail> TransactionDetails { get; set; }
    }
}
