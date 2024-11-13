namespace Domain.Entities;

public class TransactionDetail
{
    public int Id { get; set; }
    public int TransactionId { get; set; }
    public int? SparePartId { get; set; }
    public int? EquipmentId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public bool IsDeleted { get; set; }
    public Transaction Transactions { get; set; }
    public SparePart SparePart { get; set; }
    public Equipment Equipment { get; set; }
}
