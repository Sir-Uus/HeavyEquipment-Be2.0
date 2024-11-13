using Application.Dtos;
using Application.Feedbacks.Commands.CreateFeedbacks;
using Application.Feedbacks.Commands.DeleteFeedbacks;
using Application.Feedbacks.Commands.UpdateFeedbacks;
using Application.Feedbacks.Queries.GetFeedbacks;
using Application.Feedbacks.Queries.GetFeedbacksByEquipmentId;
using Application.Feedbacks.Queries.GetFeedbacksDetail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class FeedbacksController : BaseApiController
    {
#nullable enable
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetFeedbacks(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 6,
            [FromQuery] string? equipmentName = null,
            [FromQuery] string? userName = null,
            [FromQuery] string? feedbackDate = null,
            [FromQuery] int? rating = null,
            [FromQuery] string? comment = null
        )
        {
            var result = await Mediator.Send(
                new GetFeedbacksQuery.Query
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    EquipmentName = equipmentName,
                    UserName = userName,
                    FeedbackDate = feedbackDate,
                    Rating = rating,
                    Comment = comment
                }
            );
            return HandlePaginatedResult(result);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetFeedback(int id)
        {
            return HandleRegularResult(
                await Mediator.Send(new GetFeedbacksDetailQuery.Query { Id = id })
            );
        }

        [AllowAnonymous]
        [HttpGet("ByEquipment/{equipmentId}")]
        public async Task<IActionResult> GetFeedbackByEquipmentId(int EquipmentId)
        {
            return HandleRegularResult(
                await Mediator.Send(
                    new GetFeedbacksByEquipmentIdQuery.Query { EquipmentId = EquipmentId }
                )
            );
        }

        [HttpPost]
        public async Task<ActionResult> CreateFeedbacks(FeedbackDto feedbackDto)
        {
            return HandleRegularResult(
                await Mediator.Send(
                    new CreateFeedbacksCommand.Command { FeedbackDto = feedbackDto }
                )
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFeedbacks(int id, FeedbackDto feedbackDto)
        {
            feedbackDto.Id = id;

            return HandleRegularResult(
                await Mediator.Send(
                    new UpdateFeedbacksCommand.Command { FeedbackDto = feedbackDto }
                )
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedbacks(int id)
        {
            return HandleRegularResult(
                await Mediator.Send(new DeleteFeedbacksCommand.Command { Id = id })
            );
        }
    }
}
