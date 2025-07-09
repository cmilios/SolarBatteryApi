using SPCS.Application.Concurrency.Abstractions;
using SPCS.Concurrency.Models;

namespace SPCS.Application.Concurrency.DomainServices
{
    public class ConcurrencyCalculator : IConcurrencyCalculator
    {
        public ConcurrencyCalculation Calculate(Battery battery, IEnumerable<TimestampValue> timestamps)
        {
            var result = new ConcurrencyCalculation();
            foreach (var moment in timestamps)
            {
                var difference = moment.ProductionValue - moment.ConsumptionValue;
                if (moment.ConsumptionValue > 0)
                {
                    if (difference > 0)
                    {
                        battery.Charge(new PowerTimestamp
                        {
                            Date = moment.Date,
                            Value = difference
                        }, result);

                    }
                    else
                    {
                        battery.Discharge(new PowerTimestamp
                        {
                            Date = moment.Date,
                            Value = Math.Abs(difference)
                        }, result);
                    }
                }
                else
                {
                    battery.Charge(new PowerTimestamp
                    {
                        Date = moment.Date,
                        Value = moment.ProductionValue
                    }, result);
                }
            }
            return result;
        }
    }
}
