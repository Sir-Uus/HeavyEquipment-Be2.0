using Application.Core;
using Application.Dtos;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.Users.Queries.GetUsers
{
    public class GetUsersQuery
    {
        public class Query : IRequest<Result<List<UserSummaryDto>>> { }

        public class Handler : IRequestHandler<Query, Result<List<UserSummaryDto>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<List<UserSummaryDto>>> Handle(
                Query request,
                CancellationToken cancellationToken
            )
            {
                var user = await _context.Users.AsNoTracking().ToListAsync(cancellationToken);

                var userReturn = _mapper.Map<List<UserSummaryDto>>(user);

                return Result<List<UserSummaryDto>>.Success(userReturn);
            }
        }
    }
}
