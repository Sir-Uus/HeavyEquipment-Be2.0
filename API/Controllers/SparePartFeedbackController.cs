using Application.Dtos;
using Application.SparePartFeedbacks.Command.CreateSparePartFeedback;
using Application.SparePartFeedbacks.Command.DeleteSparePartFeedback;
using Application.SparePartFeedbacks.Command.UpdateSparePartFeedback;
using Application.SparePartFeedbacks.Queries.GetSparePartFeedback;
using Application.SparePartFeedbacks.Queries.GetSparePartFeedbackBySparePart;
using Application.SparePartFeedbacks.Queries.GetSparePartFeedbackDetail;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class SparePartFeedbackController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetSparePartFeedback(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 6
    )
    {
        return HandlePaginatedResult(
            await Mediator.Send(
                new GetSparePartFeedbackQuery.Query { PageNumber = pageNumber, PageSize = pageSize }
            )
        );
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSparePartFeedbackDetail(int id)
    {
        return HandleRegularResult(
            await Mediator.Send(new GetSparePartFeedbackDetailQuery.Query { Id = id })
        );
    }

    [HttpGet("by-sparepart/{sparepartId}")]
    public async Task<IActionResult> GetSparePartFeedbackDetailBySparePart(int sparepartId)
    {
        return HandleRegularResult(
            await Mediator.Send(
                new GetSparePartFeedbackBySparePartQuery.Query { SparePartId = sparepartId }
            )
        );
    }

    [HttpPost]
    public async Task<IActionResult> CreateSparePartFeedback(
        SparePartFeedbackDto sparePartFeedbackDto
    )
    {
        return HandleRegularResult(
            await Mediator.Send(
                new CreateSparePartFeedbackCommand.Command
                {
                    SparePartFeedbackDto = sparePartFeedbackDto
                }
            )
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSparePartFeedback(
        int id,
        SparePartFeedbackDto sparePartFeedbackDto
    )
    {
        sparePartFeedbackDto.Id = id;
        return HandleRegularResult(
            await Mediator.Send(
                new UpdateSparePartFeedbackCommand.Command
                {
                    SparePartFeedbackDto = sparePartFeedbackDto
                }
            )
        );
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSparePartFeedback(int id)
    {
        return HandleRegularResult(
            await Mediator.Send(new DeleteSparePartFeedbackCommand.Command { Id = id })
        );
    }
}
