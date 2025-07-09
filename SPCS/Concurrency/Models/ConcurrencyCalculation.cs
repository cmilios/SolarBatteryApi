namespace SPCS.Concurrency.Models
{
    public record ConcurrencyCalculation
    {
        public int Id { get; init; }
        public List<PowerTimestamp> BatteryHistory { get; set; } = default!;
        public List<PowerTimestamp> PowerToTheNetwork { get; set; } = default!;
        public List<PowerTimestamp> PowerFromTheNetwork { get; set; } = default!;
        public decimal ConcurrencyMetric { get; set; }
    }
}
