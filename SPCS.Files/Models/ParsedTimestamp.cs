using System;

namespace SPCS.Files.Models
{
    public class ParsedTimestamp
    {
        public int Id { get; set; }
        public DateTimeOffset Date { get; set; }
        public decimal ProductionValue { get; set; }
        public decimal ConsumptionValue { get; set; }

        public int FileId { get; set; }
        public File File { get; set; } = default!;
    }
}
