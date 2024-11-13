using Application.Dtos;
using Application.SpareParts.Command.CreateSparePart;
using Application.SpareParts.Command.DeleteSparePart;
using Application.SpareParts.Command.UpdateSparePart;
using Application.SpareParts.Queries.GetSparePart;
using Application.SpareParts.Queries.GetSparePartAll;
using Application.SpareParts.Queries.GetSparePartDetails;
using Application.SpareParts.Queries.GetSparePartImage;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class SparePartController : BaseApiController
    {
#nullable enable
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GetSpareParts(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 6,
            [FromQuery] string? equipmentName = null,
            [FromQuery] string? partName = null,
            [FromQuery] string? partNumber = null,
            [FromQuery] string? manufacturer = null,
            [FromQuery] string? availabilityStatus = null,
            [FromQuery] decimal? price = null,
            [FromQuery] decimal? minPrice = null,
            [FromQuery] decimal? maxPrice = null
        )
        {
            return HandlePaginatedResult(
                await Mediator.Send(
                    new GetSparePartQuery.Query
                    {
                        PageNumber = pageNumber,
                        PageSize = pageSize,
                        EquipmentName = equipmentName,
                        PartName = partName,
                        PartNumber = partNumber,
                        Manufacturer = manufacturer,
                        AvailabilityStatus = availabilityStatus,
                        Price = price,
                        MinPrice = minPrice,
                        MaxPrice = maxPrice
                    }
                )
            );
        }

        [HttpGet("all")]
        public async Task<ActionResult> GetSparePartsAll()
        {
            return HandleRegularResult(await Mediator.Send(new GetSparePartAllQuery.Query()));
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<SparePart>> GetSparePart(int id)
        {
            return HandleRegularResult(
                await Mediator.Send(new GetSparePartDetailsQuery.Query { Id = id })
            );
        }

        [AllowAnonymous]
        [HttpGet("image/{sparepartId}")]
        public async Task<IActionResult> GetSparePartImages(int sparepartId)
        {
            return HandleRegularResult(
                await Mediator.Send(new GetSparePartImageQuery.Query { SparePartId = sparepartId })
            );
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> CreateSparePart(SparePartsDto sparePartsDto)
        {
            return HandleRegularResult(
                await Mediator.Send(
                    new CreateSparePartCommand.Command { SparePartsDto = sparePartsDto }
                )
            );
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSparePart(int id, SparePartsDto sparePartsDto)
        {
            sparePartsDto.Id = id;
            return HandleRegularResult(
                await Mediator.Send(
                    new UpdateSparePartCommand.Command { SparePartsDto = sparePartsDto }
                )
            );
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSparePart(int id)
        {
            return HandleRegularResult(
                await Mediator.Send(new DeleteSparePartCommand.Command { Id = id })
            );
        }
    }
}
