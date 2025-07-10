using MediatR;
using SPCS.Application.Concurrency.Abstractions;
using SPCS.Concurrency.Dtos;
using SPCS.Concurrency.Mappers;
using SPCS.Concurrency.Models;

namespace SPCS.Application.Concurrency.Commands
{

    public class CreateConcurrencyCalculationCommand : IRequest<ConcurrencyCalculationDto?>
    {
        public decimal BatteryInitialState { get; set; }
        public decimal BatteryLowestThreshold { get; set; }
        public decimal BatteryHighestThreshold { get; set; }
        public decimal BatteryChargingRate { get; set; }
        public decimal BatteryDischargingRate { get; set; }
        public decimal BatteryCapacity { get; set; }
        public List<TimestampValueDto> Timestamps { get; set; } = default!;

    }

    public class CreateConcurrencyCalculationCommandHandler(IConcurrencyCalculator concurrencyCalculator, IConcurrencyCalculationRepository concurrencyCalculationRepository) : IRequestHandler<CreateConcurrencyCalculationCommand, ConcurrencyCalculationDto?>
    {
        public readonly IConcurrencyCalculator _concurrencyCalculator = concurrencyCalculator ?? throw new ArgumentNullException(nameof(concurrencyCalculator));
        public readonly IConcurrencyCalculationRepository _concurrencyCalculationRepository = concurrencyCalculationRepository ?? throw new ArgumentNullException(nameof(concurrencyCalculationRepository));

        public async Task<ConcurrencyCalculationDto?> Handle(CreateConcurrencyCalculationCommand request, CancellationToken cancellationToken)
        {
            var battery = Battery.CreateNew(
                lowestThreshold: request.BatteryLowestThreshold,
                highestThreshold: request.BatteryHighestThreshold,
                initialState: request.BatteryInitialState,
                capacity: request.BatteryCapacity,
                chargingRate: request.BatteryChargingRate,
                dischargingRate: request.BatteryDischargingRate);

            var calculatorResponse = _concurrencyCalculator
                .Calculate(battery, request.Timestamps.Select(x => TimestampValueDtoMapper.Map(x)));

            await _concurrencyCalculationRepository.AddAsync(calculatorResponse);

            var result = ConcurrencyCalculationDtoMapper.Map(calculatorResponse);
            return result;
        }
    }
}
