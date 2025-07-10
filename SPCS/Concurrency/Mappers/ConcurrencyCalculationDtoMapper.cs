using SPCS.Concurrency.Dtos;
using SPCS.Concurrency.Enum;
using SPCS.Concurrency.Models;

namespace SPCS.Concurrency.Mappers
{
    public static class ConcurrencyCalculationDtoMapper
    {
        public static ConcurrencyCalculationDto Map(ConcurrencyCalculation source)
        {
            return new ConcurrencyCalculationDto
            {
                BatteryHistory = [.. source.PowerTimestamps.Where(x => x.Type == PowerTimestampType.BatteryHistory).Select(x => PowerTimestampDtoMapper.Map(x))],
                PowerFromTheNetwork = [.. source.PowerTimestamps.Where(x => x.Type == PowerTimestampType.PowerFromTheNetwork).Select(x => PowerTimestampDtoMapper.Map(x))],
                PowerToTheNetwork = [.. source.PowerTimestamps.Where(x => x.Type == PowerTimestampType.PowerToTheNetwork).Select(x => PowerTimestampDtoMapper.Map(x))],
                ConcurrencyMetric = source.ConcurrencyMetric,
                Id = source.Id,

            };
        }
    }
}
