namespace Application.Dtos
{
    public class SparePartsDto
    {
        public int Id { get; set; }
        public int EquipmentId { get; set; }
        public string PartName { get; set; }
        public string PartNumber { get; set; }
        public string Manufacturer { get; set; }
        public string AvailabilityStatus { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public SparePartImageDto SparePartImage { get; set; }
    }
}
