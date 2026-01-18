using MediatR;
using SPCS.Application.Concurrency.Abstractions;
using SPCS.Application.Files.Abstractions;
using SPCS.Concurrency.Dtos;
using SPCS.Concurrency.Mappers;
using SPCS.Concurrency.Models;

namespace SPCS.Application.Concurrency.Commands
{

    public class CreateConcurrencyCalculationCommand : IRequest<ConcurrencyCalculationDto?>
    {
        public int ApplicationId { get; set; }
        public decimal BatteryInitialState { get; set; }
        public decimal BatteryLowestThreshold { get; set; }
        public decimal BatteryHighestThreshold { get; set; }
        public decimal BatteryChargingRate { get; set; }
        public decimal BatteryDischargingRate { get; set; }
        public decimal BatteryCapacity { get; set; }

    }

    public class CreateConcurrencyCalculationCommandHandler(IConcurrencyCalculator concurrencyCalculator, IConcurrencyCalculationRepository concurrencyCalculationRepository, IFileRepository fileRepository) : IRequestHandler<CreateConcurrencyCalculationCommand, ConcurrencyCalculationDto?>
    {
        public readonly IConcurrencyCalculator _concurrencyCalculator = concurrencyCalculator ?? throw new ArgumentNullException(nameof(concurrencyCalculator));
        public readonly IConcurrencyCalculationRepository _concurrencyCalculationRepository = concurrencyCalculationRepository ?? throw new ArgumentNullException(nameof(concurrencyCalculationRepository));
        public readonly IFileRepository _fileRepository = fileRepository ?? throw new ArgumentNullException(nameof(fileRepository));

        public async Task<ConcurrencyCalculationDto?> Handle(CreateConcurrencyCalculationCommand request, CancellationToken cancellationToken)
        {
            // Fetch parsed timestamps from files associated with the application
            var parsedTimestamps = await _fileRepository.GetParsedTimestampsByApplicationIdAsync(request.ApplicationId);

            if (!parsedTimestamps.Any())
            {
                throw new InvalidOperationException($"No parsed timestamps found for application {request.ApplicationId}. Please ensure files have been uploaded and parsed.");
            }

            // Group parsed timestamps by date and merge production and consumption values
            var timestamps = parsedTimestamps
                .GroupBy(p => p.Date)
                .Select(g => new TimestampValueDto
                {
                    Date = g.Key,
                    ProductionValue = g.Sum(x => x.ProductionValue),
                    ConsumptionValue = g.Sum(x => x.ConsumptionValue)
                })
                .OrderBy(t => t.Date)
                .ToList();

            var battery = Battery.CreateNew(
                lowestThreshold: request.BatteryLowestThreshold,
                highestThreshold: request.BatteryHighestThreshold,
                initialState: request.BatteryInitialState,
                capacity: request.BatteryCapacity,
                chargingRate: request.BatteryChargingRate,
                dischargingRate: request.BatteryDischargingRate);

            var calculatorResponse = _concurrencyCalculator
                .Calculate(battery, timestamps.Select(x => TimestampValueDtoMapper.Map(x)));

            await _concurrencyCalculationRepository.AddAsync(calculatorResponse);

            var result = ConcurrencyCalculationDtoMapper.Map(calculatorResponse);
            return result;
        }
    }
}
