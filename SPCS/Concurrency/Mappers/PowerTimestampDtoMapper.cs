using SPCS.Concurrency.Dtos;
using SPCS.Concurrency.Models;

namespace SPCS.Concurrency.Mappers
{
    public static class PowerTimestampDtoMapper
    {
        public static PowerTimestampDto Map(PowerTimestamp source)
        {
            return new PowerTimestampDto
            {
                Id = source.Id,
                Date = source.Date,
                Type = source.Type,
                Value = source.Value

            };
        }
    }
}
