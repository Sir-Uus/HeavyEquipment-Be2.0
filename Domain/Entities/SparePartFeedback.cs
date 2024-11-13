namespace Domain.Entities;

public class SparePartFeedback
{
    public int Id { get; set; }
    public int SparePartId { get; set; }
    public string UserId { get; set; }
    public DateTime FeedbackDate { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
    public bool IsDeleted { get; set; }
    public User User { get; set; }
    public SparePart SparePart { get; set; }
}
