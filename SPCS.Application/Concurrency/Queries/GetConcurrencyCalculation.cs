using MediatR;
using SPCS.Application.Concurrency.Abstractions;
using SPCS.Concurrency.Dtos;
using SPCS.Concurrency.Mappers;

namespace SPCS.Application.Concurrency.Queries
{
    public class GetConcurrencyCalculationQuery : IRequest<ConcurrencyCalculationDto>
    {
        public int Id { get; set; }
    }

    public class GetConcurrencyCalculationQueryHandler : IRequestHandler<GetConcurrencyCalculationQuery, ConcurrencyCalculationDto?>
    {
        private readonly IConcurrencyCalculationRepository _concurrencyCalculationRepository;
        public GetConcurrencyCalculationQueryHandler(IConcurrencyCalculationRepository concurrencyCalculationRepository)
        {
            _concurrencyCalculationRepository = concurrencyCalculationRepository ?? throw new ArgumentNullException(nameof(concurrencyCalculationRepository));
        }
        public async Task<ConcurrencyCalculationDto?> Handle(GetConcurrencyCalculationQuery request, CancellationToken cancellationToken)
        {

            var response = await _concurrencyCalculationRepository.GetByIdAsync(request.Id);
            if (response == null)
            {
                return null;
            }
            var result = ConcurrencyCalculationDtoMapper.Map(response);
            return result;
        }
    }
}

