using Application.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        private IMediator _mediator;

        protected IMediator Mediator =>
            _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        protected ActionResult HandlePaginatedResult<T>(Result<PaginatedList<T>> result)
        {
            if (result == null)
                return NotFound();

            if (result.IsSuccess && result.Value != null)
            {
                var paginatedList = result.Value;

                return Ok(
                    new
                    {
                        data = paginatedList,
                        totalPages = paginatedList.TotalPages,
                        currentPage = paginatedList.CurrentPage,
                        hasPreviousPage = paginatedList.HasPreviousPage,
                        hasNextPage = paginatedList.HasNextPage
                    }
                );
            }

            if (result.IsSuccess && result.Value == null)
                return NotFound();

            return BadRequest(result.Error);
        }

        protected ActionResult HandleRegularResult<T>(Result<T> result)
        {
            if (result == null)
                return NotFound();

            if (result.IsSuccess && result.Value != null)
            {
                return Ok(result.Value);
            }

            if (result.IsSuccess && result.Value == null)
                return NotFound();

            return BadRequest(result.Error);
        }
    }
}
