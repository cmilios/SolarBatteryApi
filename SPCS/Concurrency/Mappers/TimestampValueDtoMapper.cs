using SPCS.Concurrency.Dtos;
using SPCS.Concurrency.Models;

namespace SPCS.Concurrency.Mappers
{
    public static class TimestampValueDtoMapper
    {
        public static TimestampValueDto Map(TimestampValue source)
        {
            return new TimestampValueDto
            {
                Date = source.Date,
                ConsumptionValue = source.ConsumptionValue,
                ProductionValue = source.ProductionValue
            };
        }

        public static TimestampValue Map(TimestampValueDto source)
        {
            return new TimestampValue
            {
                Date = source.Date,
                ConsumptionValue = source.ConsumptionValue,
                ProductionValue = source.ProductionValue
            };
        }
    }
}
