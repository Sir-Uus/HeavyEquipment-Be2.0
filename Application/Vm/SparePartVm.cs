using Application.Dtos;

namespace Application.Vm
{
    public class SparePartVm
    {
        public int Id { get; set; }
        public int EquipmentId { get; set; }
        public string PartName { get; set; }
        public string PartNumber { get; set; }
        public string Manufacturer { get; set; }
        public string AvailabilityStatus { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public List<SparePartFeedbackVm> SparePartFeedbacks { get; set; }
        public SparePartImageDetailsVm Image { get; set; }
        public EquipmentSummaryDto Equipment { get; set; }
    }
}
