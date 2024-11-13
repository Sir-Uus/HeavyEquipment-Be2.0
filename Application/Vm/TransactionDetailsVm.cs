using System.Transactions;

namespace Application.Vm;

public class TransactionDetailsVm
{
    public int Id { get; set; }
    public int TransactionId { get; set; }
    public int? SparePartId { get; set; }
    public int? EquipmentId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public TransactionSummaryDetailsVm Transactions { get; set; }
    public SparePartVm SparePart { get; set; } = default!;
    public EquipmentVm Equipment { get; set; } = default!;
}
