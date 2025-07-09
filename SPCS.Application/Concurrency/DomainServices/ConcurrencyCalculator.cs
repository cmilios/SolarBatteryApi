using SPCS.Application.Concurrency.Abstractions;
using SPCS.Concurrency.Models;

namespace SPCS.Application.Concurrency.DomainServices
{
    public class ConcurrencyCalculator : IConcurrencyCalculator
    {
        public ConcurrencyCalculation Calculate(Battery battery, IEnumerable<TimestampValue> timestamps)
        {
            var result = new ConcurrencyCalculation();
            var concurrencyPercentageList = new List<decimal>();
            foreach (var moment in timestamps)
            {
                var difference = moment.ProductionValue - moment.ConsumptionValue;
                if (moment.ConsumptionValue > 0)
                {
                    if (difference > 0)
                    {
                        var chargeChange = battery.ChangeCurrentCharge(result.BatteryHistory, new PowerTimestamp
                        {
                            Value = difference,
                            Date = moment.Date
                        });
                        if (difference > chargeChange)
                        {
                            result.PowerToTheNetwork.Add(new PowerTimestamp
                            {
                                Date = moment.Date,
                                Value = difference - chargeChange
                            });
                        }
                        concurrencyPercentageList.Add(1);
                    }
                    else
                    {
                        var chargeChange = battery.ChangeCurrentCharge(result.BatteryHistory, new PowerTimestamp
                        {
                            Value = difference,
                            Date = moment.Date
                        });
                        if (chargeChange > difference)
                        {
                            result.PowerFromTheNetwork.Add(new PowerTimestamp
                            {
                                Date = moment.Date,
                                Value = chargeChange - difference
                            });
                        }
                        concurrencyPercentageList.Add((moment.ProductionValue + Math.Abs(chargeChange)) / moment.ConsumptionValue);
                    }
                }
                else
                {
                    var changeCharge = battery.ChangeCurrentCharge(result.BatteryHistory, new PowerTimestamp
                    {
                        Value = difference,
                        Date = moment.Date
                    });
                    if (moment.ProductionValue > changeCharge)
                    {
                        result.PowerToTheNetwork.Add(new PowerTimestamp
                        {
                            Date = moment.Date,
                            Value = moment.ProductionValue - changeCharge
                        });
                    }
                }
            }
            result.ConcurrencyMetric = concurrencyPercentageList.Count() != 0 ? concurrencyPercentageList.Average() : 0;
            return result;
        }
    }
}
