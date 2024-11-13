using Application.Dtos;
using Application.Transactions.Command.CreateTransaction;
using Application.Transactions.Command.DeleteTransaction;
using Application.Transactions.Command.UpdateTransaction;
using Application.Transactions.Queries.GetTransactionAll;
using Application.Transactions.Queries.GetTransactionByUser;
using Application.Transactions.Queries.GetTransactionDetailsQuery;
using Application.Transactions.Queries.GetTransactionQuery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class TransactionController : BaseApiController
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetTransactions(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 6
        )
        {
            return HandlePaginatedResult(
                await Mediator.Send(
                    new GetTransactionQuery.Query { PageNumber = pageNumber, PageSize = pageSize }
                )
            );
        }

        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<IActionResult> GetTransactionAll()
        {
            return HandleRegularResult(await Mediator.Send(new GetTransactionAllQuery.Query()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransaction(int id)
        {
            return HandleRegularResult(
                await Mediator.Send(new GetTransactionDetailsQuery.Query { Id = id })
            );
        }

        [HttpGet("by-user/{userId}")]
        public async Task<IActionResult> GetTransactionByUser(
            string userId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 6
        )
        {
            return HandlePaginatedResult(
                await Mediator.Send(
                    new GetTransactionByUserQuery.Query
                    {
                        UserId = userId,
                        PageNumber = pageNumber,
                        PageSize = pageSize
                    }
                )
            );
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction(TransactionDto transactionDto)
        {
            return HandleRegularResult(
                await Mediator.Send(
                    new CreateTransactionCommand.Command { TransactionDto = transactionDto }
                )
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditTransaction(int id, TransactionDto transactionDto)
        {
            transactionDto.Id = id;
            return HandleRegularResult(
                await Mediator.Send(
                    new UpdateTransactionCommand.Command { TransactionDto = transactionDto }
                )
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            return HandleRegularResult(
                await Mediator.Send(new DeleteTransactionCommand.Command { Id = id })
            );
        }
    }
}
