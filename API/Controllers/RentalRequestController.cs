using Application.Dtos;
using Application.RentalRequests.Command.CreateRentalRequest;
using Application.RentalRequests.Command.DeleteRentalRequest;
using Application.RentalRequests.Command.UpdateRentalRequest;
using Application.RentalRequests.Queries.GetRentalRequest;
using Application.RentalRequests.Queries.GetRentalRequestAll;
using Application.RentalRequests.Queries.GetRentalRequestByUser;
using Application.RentalRequests.Queries.GetRentalRequestDetails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class RentalRequestController : BaseApiController
    {
#nullable enable

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetRentalRequest(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 6,
            [FromQuery] string? status = null,
            [FromQuery] string? userName = null,
            [FromQuery] string? equipmentName = null,
            [FromQuery] string? invoice = null,
            [FromQuery] string? starDate = null,
            [FromQuery] string? endDate = null,
            [FromQuery] string? paymentStatus = null
        )
        {
            return HandlePaginatedResult(
                await Mediator.Send(
                    new GetRentalRequestQuery.Query
                    {
                        PageNumber = pageNumber,
                        PageSize = pageSize,
                        Status = status,
                        UserName = userName,
                        EquipmentName = equipmentName,
                        Invoice = invoice,
                        StarDate = starDate,
                        EndDate = endDate,
                        PaymentStatus = paymentStatus
                    }
                )
            );
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetRentalRequestaLL()
        {
            return HandleRegularResult(await Mediator.Send(new GetRentalRequestAllQuery.Query()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRentalRequests(int id)
        {
            return HandleRegularResult(
                await Mediator.Send(new GetRentalRequestDetailsQuery.Query { Id = id })
            );
        }

        [HttpGet("by-user/{userId}")]
        public async Task<IActionResult> GetRentalRequestByUser(
            string userId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 6,
            [FromQuery] string? paymentStatus = null
        )
        {
            return HandlePaginatedResult(
                await Mediator.Send(
                    new GetRentalRequestByUserQuery.Query
                    {
                        UserId = userId,
                        PageNumber = pageNumber,
                        PageSize = pageSize,
                        PaymentStatus = paymentStatus
                    }
                )
            );
        }

        [HttpPost]
        public async Task<IActionResult> CreateRentalRequest(RentalRequestDto rentalRequestDto)
        {
            return HandleRegularResult(
                await Mediator.Send(
                    new CreateRentalRequestCommand.Command { RentalRequestDto = rentalRequestDto }
                )
            );
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRentalRequest(
            int id,
            RentalRequestDto rentalRequestDto
        )
        {
            rentalRequestDto.Id = id;
            return HandleRegularResult(
                await Mediator.Send(
                    new UpdateRentalRequestCommand.Command { RentalRequestDto = rentalRequestDto }
                )
            );
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRentalRequest(int id)
        {
            return HandleRegularResult(
                await Mediator.Send(new DeleteRentalRequestCommand.Command { Id = id })
            );
        }
    }
}
