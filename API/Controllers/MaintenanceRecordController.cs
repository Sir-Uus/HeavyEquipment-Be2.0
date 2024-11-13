using Application.Dtos;
using Application.MaintenanceRecord.Command.CreateMaintenanceRecord;
using Application.MaintenanceRecord.Command.DeleteMaintenanceRecord;
using Application.MaintenanceRecord.Command.UpdateMaintenanceRecord;
using Application.MaintenanceRecord.Queries.GetMaintenanceRecord;
using Application.MaintenanceRecord.Queries.GetMaintenanceRecordDetails;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class MaintenanceRecordController : BaseApiController
    {
#nullable enable
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<List<MaintenancedRecord>>> GetMaintenanceRecords(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 6,
            [FromQuery] string? equipmentName = null,
            [FromQuery] string? maintenanceDate = null,
            [FromQuery] string? servicedPerformed = null,
            [FromQuery] string? servicedProvider = null,
            [FromQuery] decimal? cost = null,
            [FromQuery] string? nextMaintenanceDue = null
        )
        {
            return HandlePaginatedResult(
                await Mediator.Send(
                    new GetMaintenanceRecordQuery.Query
                    {
                        PageNumber = pageNumber,
                        PageSize = pageSize,
                        EquipmentName = equipmentName,
                        MaintenanceDate = maintenanceDate,
                        ServicedPerformed = servicedPerformed,
                        ServiceProfider = servicedProvider,
                        Cost = cost,
                        NextMaintenanceDue = nextMaintenanceDue
                    }
                )
            );
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetMaintenanceRecord(int id)
        {
            return HandleRegularResult(
                await Mediator.Send(new GetMaintenanceRecordDetailsQuery.Query { Id = id })
            );
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> CreateMaintenanceRecord(
            MaintenanceRecordDto maintenanceRecordDto
        )
        {
            return HandleRegularResult(
                await Mediator.Send(
                    new CreateMaintenanceRecordCommand.Command
                    {
                        MaintenanceRecordDto = maintenanceRecordDto
                    }
                )
            );
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMaintenanceRecord(
            int id,
            MaintenanceRecordDto maintenanceRecordDto
        )
        {
            maintenanceRecordDto.Id = id;
            return HandleRegularResult(
                await Mediator.Send(
                    new UpdateMaintenanceRecordCommand.Command
                    {
                        MaintenanceRecordDto = maintenanceRecordDto
                    }
                )
            );
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaintenanceRecord(int id)
        {
            return HandleRegularResult(
                await Mediator.Send(new DeleteMaintenanceRecordCommand.Command { Id = id })
            );
        }
    }
}
