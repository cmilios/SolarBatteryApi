using MediatR;
using Microsoft.AspNetCore.Mvc;
using SPCS.Application.Applications.Commands;
using SPCS.Application.Applications.Queries;
using SPCS.Application.Concurrency.Commands;
using SPCS.Application.Files.Abstractions;
using SPCS.Concurrency.Dtos;
using System.Linq;

namespace SPCS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateApplicationCommand command, CancellationToken cancellationToken)
        {
            var app = await _mediator.Send(command, cancellationToken);
            return Ok(app);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var query = new GetApplicationsQuery();
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var query = new GetApplicationByIdQuery { Id = id };
            var app = await _mediator.Send(query, cancellationToken);
            if (app == null) return NotFound();
            return Ok(app);
        }

        [HttpPost("{id}/calculate")]
        public async Task<IActionResult> Calculate(int id, [FromBody] CreateConcurrencyCalculationCommand command, CancellationToken cancellationToken)
        {
            // Set the ApplicationId from the route parameter
            command.ApplicationId = id;

            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
    }
}
