namespace Application.Dtos
{
    public class RentalHistoryDto
    {
        public int Id { get; set; }
        public int EquipmentId { get; set; }
        public string RenterId { get; set; }
        public string Invoice { get; set; }
        public DateTime RentalStartDate { get; set; }
        public DateTime RentalEndDate { get; set; }
        public decimal RentalCost { get; set; }
        public string Location { get; set; }
    }
}
