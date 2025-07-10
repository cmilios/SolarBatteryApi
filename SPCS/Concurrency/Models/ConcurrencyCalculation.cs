namespace SPCS.Concurrency.Models
{
    public record ConcurrencyCalculation
    {
        public int Id { get; init; }
        public List<PowerTimestamp> PowerTimestamps { get; set; } = new();
        public decimal ConcurrencyMetric { get; set; }
    }
}
