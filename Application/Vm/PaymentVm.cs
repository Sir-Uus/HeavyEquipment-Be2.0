
namespace Application.Vm
{
    public class PaymentVm
    {
        public int Id { get; set; }
        public int RentalRequestId { get; set; }
        public int TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime PaymentDate { get; set; }
        public RentalRequestVm RentalRequest { get; set; }

    }
}