using Application.Core;
using Application.Dtos;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.Users.Queries.GetUserDetails
{
    public class GetUserDetailsQuery
    {
        public class Query : IRequest<Result<UserSummaryDto>>
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<UserSummaryDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<UserSummaryDto>> Handle(
                Query request,
                CancellationToken cancellationToken
            )
            {
                var user = await _context
                    .Users.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == request.Id);

                var userReturn = _mapper.Map<UserSummaryDto>(user);

                return Result<UserSummaryDto>.Success(userReturn);
            }
        }
    }
}
