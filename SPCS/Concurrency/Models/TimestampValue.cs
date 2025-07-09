namespace SPCS.Concurrency.Models
{
    public class TimestampValue
    {
        public DateTimeOffset Date { get; set; }

        public decimal ProductionValue { get; set; }
        public decimal ConsumptionValue { get; set; }
    }
}
