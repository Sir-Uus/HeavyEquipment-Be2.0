namespace Application.Dtos;

public class TransactionDto
{
    public int Id { get; set; }
    public string Invoice { get; set; } = default!;
    public string UserId { get; set; } = default!;
    public DateTime TransactionDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = default!;
}
