using Application.Dtos;
using Application.RentalHistories.Command.CreateRentalHistories;
using Application.RentalHistories.Command.DeleteRentalHistories;
using Application.RentalHistories.Command.UpdateRentalHistories;
using Application.RentalHistories.Queries.GetRentalHistories;
using Application.RentalHistories.Queries.GetRentalHistoriesByEquipmentId;
using Application.RentalHistories.Queries.GetRentalHistoriesDetails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class RentalHistoryController : BaseApiController
    {
#nullable enable
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetRentalHistories(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 6,
            [FromQuery] string? equipmentName = null,
            [FromQuery] string? renter = null,
            [FromQuery] string? invoice = null,
            [FromQuery] string? rentalStartDate = null,
            [FromQuery] string? rentalEndDate = null,
            [FromQuery] decimal? rentalCost = null,
            [FromQuery] string? location = null
        )
        {
            return HandlePaginatedResult(
                await Mediator.Send(
                    new GetRentalHistoriesQuery.Query
                    {
                        PageNumber = pageNumber,
                        PageSize = pageSize,
                        EquipmentName = equipmentName,
                        Renter = renter,
                        Invoice = invoice,
                        RentalStartDate = rentalStartDate,
                        RentalEndDate = rentalEndDate,
                        RentalCost = rentalCost,
                        Location = location
                    }
                )
            );
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetRentalHistory(int id)
        {
            return HandleRegularResult(
                await Mediator.Send(new GetRentalHistoriesDetailsQuery.Query { Id = id })
            );
        }

        [AllowAnonymous]
        [HttpGet("ByEquipment/{equipmentId}")]
        public async Task<IActionResult> GetRentalHistoryByEquipmentId(int EquipmentId)
        {
            return HandleRegularResult(
                await Mediator.Send(
                    new GetRentalHistoriesByEquipmentIdQuery.Query { EquipmentId = EquipmentId }
                )
            );
        }

        [HttpPost]
        public async Task<ActionResult> CreateRentalHistory(RentalHistoryDto rentalHistoryDto)
        {
            return HandleRegularResult(
                await Mediator.Send(
                    new CreateRentalHistoriesCommand.Command { RentalHistoryDto = rentalHistoryDto }
                )
            );
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRentalHistory(
            int id,
            RentalHistoryDto rentalHistoryDto
        )
        {
            rentalHistoryDto.Id = id;
            return HandleRegularResult(
                await Mediator.Send(
                    new UpdateRentalHistoriesCommand.Command { RentalHistoryDto = rentalHistoryDto }
                )
            );
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRentalHistory(int id)
        {
            return HandleRegularResult(
                await Mediator.Send(new DeleteRentalHistoriesCommand.Command { Id = id })
            );
        }
    }
}
