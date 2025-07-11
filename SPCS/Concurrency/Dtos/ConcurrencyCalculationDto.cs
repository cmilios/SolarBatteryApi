namespace SPCS.Concurrency.Dtos
{
    public class ConcurrencyCalculationDto
    {
        public int Id { get; init; }
        public List<PowerTimestampDto> BatteryHistory { get; set; } = default!;
        public List<PowerTimestampDto> PowerToTheNetwork { get; set; } = default!;
        public List<PowerTimestampDto> PowerFromTheNetwork { get; set; } = default!;
        public decimal ConcurrencyMetric { get; set; }
        public decimal NeedCoverage { get; set; }

    }
}
