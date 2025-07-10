using SPCS.Concurrency.Dtos;
using SPCS.Concurrency.Models;

namespace SPCS.Concurrency.Mappers
{
    public static class ConcurrencyCalculationDtoMapper
    {
        public static ConcurrencyCalculationDto Map(ConcurrencyCalculation source)
        {
            return new ConcurrencyCalculationDto
            {
                PowerTimestamps = [.. source.PowerTimestamps.Select(x => PowerTimestampDtoMapper.Map(x))],
                ConcurrencyMetric = source.ConcurrencyMetric,
                Id = source.Id,

            };
        }
    }
}
