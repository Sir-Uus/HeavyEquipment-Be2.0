namespace Domain.Entities;

public class Transaction
{
    public int Id { get; set; }
    public string Invoice { get; set; }
    public string UserId { get; set; }
    public DateTime TransactionDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; }
    public bool IsDeleted { get; set; }
    public ICollection<TransactionDetail> TransactionDetails { get; set; }
    public ICollection<Payment> Payments { get; set; }
}
