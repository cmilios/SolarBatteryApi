using SPCS.Concurrency.Enum;

namespace SPCS.Concurrency.Dtos
{
    public class PowerTimestampDto
    {
        public int Id { get; set; }
        public DateTimeOffset Date { get; set; }
        public decimal Value { get; set; }
        public PowerTimestampType Type { get; set; }
    }
}
