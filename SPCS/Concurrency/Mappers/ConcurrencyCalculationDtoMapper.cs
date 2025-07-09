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
                BatteryHistory = source.BatteryHistory,
                ConcurrencyMetric = source.ConcurrencyMetric,
                Id = source.Id,
                PowerFromTheNetwork = source.PowerFromTheNetwork,
                PowerToTheNetwork = source.PowerToTheNetwork

            };
        }
    }
}
