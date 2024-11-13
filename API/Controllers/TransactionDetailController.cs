using Application.Dtos;
using Application.TransactionDetails.Command.CreateTransactionDetail;
using Application.TransactionDetails.Command.DeleteTransactionDetails;
using Application.TransactionDetails.Queries.GetTransactionDetailByUser;
using Application.TransactionDetails.Queries.GetTransactionDetailDetails;
using Application.TransactionDetails.Queries.GetTransactionDetails;
using Application.Transactions.Queries.GetTransactionByUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class TransactionDetailController : BaseApiController
    {
#nullable enable
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetTransactionDetail(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 6
        )
        {
            return HandlePaginatedResult(
                await Mediator.Send(
                    new GetTransactionDetailsQuery.Query
                    {
                        PageNumber = pageNumber,
                        PageSize = pageSize
                    }
                )
            );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransactionDetails(int id)
        {
            return HandleRegularResult(
                await Mediator.Send(new GetTransactionDetailDetailsQuery.Query { Id = id })
            );
        }

        [HttpGet("by-user/{userId}")]
        public async Task<IActionResult> GetTransactionDetailsByUser(
            string? userId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 6,
            [FromQuery] string? status = null
        )
        {
            return HandlePaginatedResult(
                await Mediator.Send(
                    new GetTransactionDetailByUserQuery.Query
                    {
                        UserId = userId,
                        PageNumber = pageNumber,
                        PageSize = pageSize,
                        Status = status
                    }
                )
            );
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction(
            TransactionDetailsDto TransactionDetailsDto
        )
        {
            return HandleRegularResult(
                await Mediator.Send(
                    new CreateTransactionDetailCommand.Command
                    {
                        TransactionDetailsDto = TransactionDetailsDto
                    }
                )
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransactionDetail(int id)
        {
            return HandleRegularResult(
                await Mediator.Send(new DeleteTransactionDetailsCommand.Command { Id = id })
            );
        }
    }
}
