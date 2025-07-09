using SPCS.Concurrency.Models;

namespace SPCS.Application.Concurrency.Abstractions
{
    public interface IConcurrencyCalculator
    {
        ConcurrencyCalculation Calculate(Battery battery, IEnumerable<TimestampValue> timestamps);
    }
}
