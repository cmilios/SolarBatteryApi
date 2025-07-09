using MediatR;
using Microsoft.AspNetCore.Mvc;
using SPCS.Application.Concurrency.Commands;
using SPCS.Application.Concurrency.Queries;
using SPCS.Concurrency.Dtos;

namespace SPCS.API.Controllers
{
    public class ConcurrencyControler(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("{id}/get")]
        public async Task<ConcurrencyCalculationDto> GetAsync(int id, CancellationToken cancellationToken)
        {
            var query = new GetConcurrencyCalculationQuery
            {
                Id = id
            };

            return await _mediator.Send(query, cancellationToken);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCalculation([FromBody] CreateConcurrencyCalculationCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetAsync), new { id = result!.Id }, result);
        }
    }
}
