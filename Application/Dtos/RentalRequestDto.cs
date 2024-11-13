namespace Application.Dtos
{
    public class RentalRequestDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int EquipmentId { get; set; }
        public string Invoice { get; set; }
        public DateTime StarDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
    }
}
