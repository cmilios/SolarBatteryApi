using MediatR;
using SPCS.Concurrency.Dtos;
using SPCS.Concurrency.Mappers;
using SPCS.Concurrency.Models;

namespace SPCS.Application.Concurrency.Queries
{
    public class GetConcurrencyCalculationQuery : IRequest<ConcurrencyCalculationDto>
    {
        public int Id { get; set; }
    }

    public class GetConcurrencyCalculationQueryHandler : IRequestHandler<GetConcurrencyCalculationQuery, ConcurrencyCalculationDto>
    {
        public Task<ConcurrencyCalculationDto> Handle(GetConcurrencyCalculationQuery request, CancellationToken cancellationToken)
        {
            var response = new ConcurrencyCalculation();
            var result = ConcurrencyCalculationDtoMapper.Map(response);
            return Task.FromResult(result);
        }
    }
}

