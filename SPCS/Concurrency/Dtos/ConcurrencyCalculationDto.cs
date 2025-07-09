using SPCS.Concurrency.Models;

namespace SPCS.Concurrency.Dtos
{
    public class ConcurrencyCalculationDto
    {
        public int Id { get; init; }
        public List<PowerTimestamp> BatteryHistory { get; set; } = default!;
        public List<PowerTimestamp> PowerToTheNetwork { get; set; } = default!;
        public List<PowerTimestamp> PowerFromTheNetwork { get; set; } = default!;
        public decimal ConcurrencyMetric { get; set; }
    }
}
