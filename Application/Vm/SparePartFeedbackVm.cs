using System;
using Application.Dtos;

namespace Application.Vm;

public class SparePartFeedbackVm
{
    public int Id { get; set; }
    public int SparePartId { get; set; }
    public string UserId { get; set; }
    public DateTime FeedbackDate { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
    public bool IsDeleted { get; set; }
    public UserSummaryDto User { get; set; }
}
