using Application.Core;
using Application.Dtos;
using Application.SetValidator;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.Feedbacks.Commands.CreateFeedbacks
{
    public class CreateFeedbacksCommand
    {
        public class Command : IRequest<Result<Unit>>
        {
            public FeedbackDto FeedbackDto { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public class CommandValidator : AbstractValidator<Command>
            {
                public CommandValidator()
                {
                    RuleFor(x => x.FeedbackDto).SetValidator(new FeedbackValidator());
                }
            }

            public async Task<Result<Unit>> Handle(
                Command request,
                CancellationToken cancellationToken
            )
            {
                var equipment = await _context.Equipments.FirstOrDefaultAsync(x =>
                    x.Id == request.FeedbackDto.EquipmentId
                );

                var user = await _context.Users.FirstOrDefaultAsync(x =>
                    x.Id == request.FeedbackDto.UserId
                );

                if (equipment == null)
                {
                    return Result<Unit>.Failure("Equipment not found");
                }

                if (user == null)
                {
                    return Result<Unit>.Failure("User must be registered first");
                }

                var performanceFeedback = _mapper.Map<PerformanceFeedback>(request.FeedbackDto);
                _context.PerformanceFeedbacks.Add(performanceFeedback);
                var result = await _context.SaveChangesAsync() > 0;
                if (!result)
                    return Result<Unit>.Failure("Failed to create feedbacks");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
