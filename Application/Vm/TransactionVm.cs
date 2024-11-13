namespace Application.Vm;

public class TransactionVm
{
    public int Id { get; set; }
    public string Invoice { get; set; } = default!;
    public string UserId { get; set; } = default!;
    public DateTime TransactionDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = default!;
    public List<TransactionDetailsVm> TransactionDetails { get; set; } = default!;
}

public class TransactionSummaryDetailsVm
{
    public int Id { get; set; }
    public string Invoice { get; set; } = default!;
    public string UserId { get; set; } = default!;
    public DateTime TransactionDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = default!;
}
