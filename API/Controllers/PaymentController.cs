using Application.Dtos;
using Application.Payments.Command.CreatePayment;
using Application.Payments.Command.DeletePayment;
using Application.Payments.Command.UpdatePayment;
using Application.Payments.Queries.GetPayment;
using Application.Payments.Queries.GetPaymentAll;
using Application.Payments.Queries.GetPaymentByRentalRequestId;
using Application.Payments.Queries.GetPaymentDetails;
using Application.Transactions.Command.UpdateTransaction;
using Application.Transactions.Queries.GetTransactionDetailsQuery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PaymentController : BaseApiController
    {
#nullable enable
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetPAyments(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 6,
            [FromQuery] string? invoice = null,
            [FromQuery] decimal? amount = null,
            [FromQuery] string? paymentStatus = null,
            [FromQuery] string? paymentMethod = null
        )
        {
            return HandlePaginatedResult(
                await Mediator.Send(
                    new GetPaymentQuery.Query
                    {
                        PageNumber = pageNumber,
                        PageSize = pageSize,
                        Invoice = invoice,
                        Amount = amount,
                        PaymentStatus = paymentStatus,
                        PaymentMethod = paymentMethod
                    }
                )
            );
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetPAymentsAll()
        {
            return HandleRegularResult(await Mediator.Send(new GetPaymentAllQuery.Query()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPayment(int id)
        {
            return HandleRegularResult(
                await Mediator.Send(new GetPaymentDetailsQuery.Query { Id = id })
            );
        }

        [HttpGet("by-rental-request/{rentalRequestId}")]
        public async Task<IActionResult> GetPaymentByRentalRequestId(int rentalRequestId)
        {
            return HandleRegularResult(
                await Mediator.Send(
                    new GetPaymentByRentalRequestIdQuery.Query { RentalRequestId = rentalRequestId }
                )
            );
        }

        [AllowAnonymous]
        [HttpGet("complete-payment/{rentalRequestId}")]
        public async Task<IActionResult> CompletePayment(int rentalRequestId)
        {
            var paymentResult = await Mediator.Send(
                new GetPaymentByRentalRequestIdQuery.Query { RentalRequestId = rentalRequestId }
            );

            if (paymentResult == null || paymentResult.Value == null)
            {
                return NotFound("Payment not found.");
            }

            var paymentVm = paymentResult.Value;

            var paymentDto = new PaymentDto
            {
#nullable disable
                Id = paymentVm.Id,
                RentalRequestId = paymentVm?.RentalRequestId,
                Amount = paymentVm.Amount,
                PaymentStatus = "Paid",
                PaymentMethod = paymentVm.PaymentMethod
            };

            try
            {
                await Mediator.Send(new UpdatePaymentCommand.Command { PaymentDto = paymentDto });
                return Ok("Payment status updated to Paid successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    $"An error occurred while updating the payment: {ex.Message}"
                );
            }
        }

        [AllowAnonymous]
        [HttpGet("complete-payment-sparepart/{transactionId}")]
        public async Task<IActionResult> CompletePaymentSparepart(int transactionId)
        {
            var transactionResult = await Mediator.Send(
                new GetTransactionDetailsQuery.Query { Id = transactionId }
            );

            if (!transactionResult.IsSuccess || transactionResult.Value == null)
                return NotFound("Transaction not found.");

            var transactionVm = transactionResult.Value;

            var transactionDto = new TransactionDto
            {
                Id = transactionVm.Id,
                Invoice = transactionVm.Invoice,
                UserId = transactionVm.UserId,
                TransactionDate = transactionVm.TransactionDate,
                TotalAmount = transactionVm.TotalAmount,
                Status = "Paid"
            };

            var updateResult = await Mediator.Send(
                new UpdateTransactionCommand.Command { TransactionDto = transactionDto }
            );

            if (!updateResult.IsSuccess)
                return StatusCode(500, "Failed to update transaction status.");

            string htmlContent =
                "<h1>Payment Successful</h1>"
                + "<p>Your payment has been completed successfully. Thank you for your purchase!</p>";

            return Content(htmlContent, "text/html");
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment(PaymentDto paymentDto)
        {
            return HandleRegularResult(
                await Mediator.Send(new CreatePaymentCommand.Command { PaymentDto = paymentDto })
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePayment(int id, PaymentDto paymentDto)
        {
            paymentDto.Id = id;
            return HandleRegularResult(
                await Mediator.Send(new UpdatePaymentCommand.Command { PaymentDto = paymentDto })
            );
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            return HandleRegularResult(
                await Mediator.Send(new DeletePaymentCommand.Command { Id = id })
            );
        }
    }
}
