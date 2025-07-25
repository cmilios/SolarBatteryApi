using SPCS.Common.Enums;

namespace SPCS.Common.Models
{
    public class SPCSConfiguration
    {
        public int Id { get; init; }
        public string Name { get; init; } = default!;
        public string Value { get; init; } = default!;
        public SPCSConfigType Type { get; init; } = default!;

    }
}
