namespace Application.Dtos;

public class SparePartFeedbackDto
{
    public int Id { get; set; }
    public int SparePartId { get; set; }
    public string UserId { get; set; }
    public DateTime FeedbackDate { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
}
