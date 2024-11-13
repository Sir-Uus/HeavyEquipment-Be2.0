using Application.Dtos;
using Application.Equipments.Command.CreateEquipment;
using Application.Equipments.Command.DeleteEquipment;
using Application.Equipments.Command.UpdateEquipment;
using Application.Equipments.Queries.GetEquipmentAll;
using Application.Equipments.Queries.GetEquipmentDetails;
using Application.Equipments.Queries.GetEquipmentImages;
using Application.Equipments.Queries.GetEquipments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class EquipmentController : BaseApiController
    {
#nullable enable

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetEquipments(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 6,
            [FromQuery] string? searchTerm = null,
            [FromQuery] string? type = null,
            [FromQuery] string? status = null,
            [FromQuery] string? location = null,
            [FromQuery] string? brand = null,
            [FromQuery] string? model = null,
            [FromQuery] string? specification = null,
            [FromQuery] string? description = null,
            [FromQuery] string? yearOfManufacture = null,
            [FromQuery] decimal? rentalPrice = null,
            [FromQuery] int? unit = null,
            [FromQuery] decimal? minPrice = null,
            [FromQuery] decimal? maxPrice = null
        )
        {
            return HandlePaginatedResult(
                await Mediator.Send(
                    new GetEquipmentQuery.Query
                    {
                        PageNumber = pageNumber,
                        PageSize = pageSize,
                        SearchTerm = searchTerm,
                        Type = type,
                        Status = status,
                        Location = location,
                        Brand = brand,
                        Model = model,
                        Specification = specification,
                        Description = description,
                        YearOfManufacture = yearOfManufacture,
                        RentalPrice = rentalPrice,
                        Unit = unit,
                        MinPrice = minPrice,
                        MaxPrice = maxPrice,
                    }
                )
            );
        }

        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<IActionResult> GetEquipmentsAll()
        {
            return HandleRegularResult(await Mediator.Send(new GetEquipmentAllQuery.Query()));
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEquipment(int id)
        {
            return HandleRegularResult(
                await Mediator.Send(new GetEquipmentDetailQuery.Query { Id = id })
            );
        }

        [AllowAnonymous]
        [HttpGet("image/{equipmentId}")]
        public async Task<IActionResult> GetEquipmentImages(int equipmentId)
        {
            return HandleRegularResult(
                await Mediator.Send(new GetEquipmentImagesQuery.Query { EquipmentId = equipmentId })
            );
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateEquipment(EquipmentDto equipmentDto)
        {
            return HandleRegularResult(
                await Mediator.Send(
                    new CreateEquipmentCommand.Command { EquipmentDto = equipmentDto }
                )
            );
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditEquipment(int id, EquipmentDto equipmentDto)
        {
            equipmentDto.Id = id;

            return HandleRegularResult(
                await Mediator.Send(
                    new UpdateEquipmentCommand.Command { EquipmentDto = equipmentDto }
                )
            );
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return HandleRegularResult(
                await Mediator.Send(new DeleteEquipmentCommand.Command { Id = id })
            );
        }
    }
}
