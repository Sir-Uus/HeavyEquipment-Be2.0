using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class User : IdentityUser
    {
        public string DisplayName { get; set; }
        public string Contact { get; set; }
        public ICollection<PerformanceFeedback> PerformanceFeedbacks { get; set; }
        public ICollection<SparePartFeedback> SparePartFeedbacks { get; set; }
        public ICollection<RentalHistory> RentalHistories { get; set; }
        public ICollection<RentalRequest> RentalRequests { get; set; }
    }
}
