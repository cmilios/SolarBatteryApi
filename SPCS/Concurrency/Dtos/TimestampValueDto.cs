namespace SPCS.Concurrency.Dtos
{
    public class TimestampValueDto
    {
        public DateTimeOffset Date { get; set; }

        public decimal ProductionValue { get; set; }
        public decimal ConsumptionValue { get; set; }
    }
}
