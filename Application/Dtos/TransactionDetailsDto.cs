namespace Application.Dtos;

public class TransactionDetailsDto
{
    public int Id { get; set; }
    public int TransactionId { get; set; }
    public int? SparePartId { get; set; }
    public int? EquipmentId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
