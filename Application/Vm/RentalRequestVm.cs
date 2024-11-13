using Application.Dtos;

namespace Application.Vm
{
    public class RentalRequestVm
    {
        public int Id { get; set; }
        public string UserId { get; set; } = default!;
        public int EquipmentId { get; set; }
        public string Invoice { get; set; } = default!;
        public DateTime StarDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = default!;
        public List<PaymentDto> Payments { get; set; }

        // public UserSummaryDto User { get; set; }
        public EquipmentSummaryDto Equipment { get; set; }
    }
}
