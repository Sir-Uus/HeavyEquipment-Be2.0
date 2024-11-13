using Application.Dtos;

namespace Application.Vm
{
    public class FeedbackVm
    {
        public int Id { get; set; }
        public int EquipmentId { get; set; }
        public string UserId { get; set; }
        public DateTime FeedbackDate { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public EquipmentSummaryDto Equipment { get; set; }
    }
}