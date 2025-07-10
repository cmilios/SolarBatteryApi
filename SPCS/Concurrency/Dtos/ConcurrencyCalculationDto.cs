namespace SPCS.Concurrency.Dtos
{
    public class ConcurrencyCalculationDto
    {
        public int Id { get; init; }
        public List<PowerTimestampDto> PowerTimestamps { get; set; } = new();
        public decimal ConcurrencyMetric { get; set; }
    }
}
