using Application.Core;
using Application.Dtos;
using Application.Vm;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.Feedbacks.Queries.GetFeedbacks
{
    public class GetFeedbacksQuery
    {
        public class Query : IRequest<Result<PaginatedList<FeedbackVm>>>
        {
#nullable enable
            public int PageNumber { get; set; } = 1;
            public int PageSize { get; set; } = 6;
            public string? EquipmentName { get; set; }
            public string? UserName { get; set; }
            public string? FeedbackDate { get; set; }
            public int? Rating { get; set; }
            public string? Comment { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<PaginatedList<FeedbackVm>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<PaginatedList<FeedbackVm>>> Handle(
                Query request,
                CancellationToken cancellationToken
            )
            {
                var query = _context
                    .PerformanceFeedbacks.AsNoTracking()
                    .AsSplitQuery()
                    .Include(e => e.Equipment)
                    .Include(u => u.User)
                    .OrderByDescending(x => x.FeedbackDate)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(request.EquipmentName))
                {
                    var lowerCaseFeedback = request.EquipmentName.ToLower();
                    query = query.Where(f =>
                        f.Equipment.Name.ToLower().Contains(lowerCaseFeedback)
                    );
                }

                if (!string.IsNullOrWhiteSpace(request.UserName))
                {
                    var lowerCaseFeedback = request.UserName.ToLower();
                    query = query.Where(f =>
                        f.User.DisplayName.ToLower().Contains(lowerCaseFeedback)
                    );
                }

                if (!string.IsNullOrWhiteSpace(request.FeedbackDate))
                {
                    if (DateTime.TryParse(request.FeedbackDate, out var targetDate))
                    {
                        targetDate = DateTime.SpecifyKind(targetDate, DateTimeKind.Utc);

                        var startDate = targetDate.Date;
                        var endDate = startDate.AddDays(1);

                        query = query.Where(f =>
                            f.FeedbackDate >= startDate && f.FeedbackDate < endDate
                        );
                    }
                }

                if (request.Rating.HasValue)
                {
                    query = query.Where(f => f.Rating == request.Rating.Value);
                }

                if (!string.IsNullOrWhiteSpace(request.Comment))
                {
                    var lowerCaseComment = request.Comment.ToLower();
                    query = query.Where(f => f.Comment.ToLower().Contains(lowerCaseComment));
                }

                var totalItems = await query.CountAsync(cancellationToken);

                var feedbacks = await query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync(cancellationToken);

                var feedbackVmList = _mapper.Map<List<FeedbackVm>>(feedbacks);

                var paginatedList = new PaginatedList<FeedbackVm>(
                    feedbackVmList,
                    totalItems,
                    request.PageNumber,
                    request.PageSize
                );

                return Result<PaginatedList<FeedbackVm>>.Success(
                    paginatedList,
                    totalCount: paginatedList.TotalCount,
                    totalPages: paginatedList.TotalPages,
                    currentPage: paginatedList.CurrentPage,
                    pageSize: paginatedList.PageSize,
                    hasPreviousPage: paginatedList.HasPreviousPage,
                    hasNextPage: paginatedList.HasNextPage
                );
            }
        }
    }
}
