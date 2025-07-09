namespace SPCS.Concurrency.Models
{
    public class Battery
    {
        public static Battery CreateNew(
            decimal initialState,
            decimal lowestThreshold,
            decimal highestThreshold,
            decimal capacity,
            decimal chargingRate,
            decimal dischargingRate)
            => new(
                highestThreshold: highestThreshold,
                lowestThreshold: lowestThreshold,
                initialState: initialState,
                capacity: capacity,
                dischargingRate: dischargingRate,
                chargingRate: chargingRate);
        private Battery(
            decimal initialState,
            decimal lowestThreshold,
            decimal highestThreshold,
            decimal capacity,
            decimal chargingRate,
            decimal dischargingRate)
        {
            InitialState = initialState;
            LowerstThreshold = lowestThreshold;
            HighestThreshold = highestThreshold;
            Capacity = capacity;
            CurrentState = initialState;
            ChargingRate = chargingRate;
            DischargingRate = dischargingRate;
        }

        public decimal InitialState { get; set; }
        public decimal LowerstThreshold { get; set; }
        public decimal HighestThreshold { get; set; }
        public decimal ChargingRate { get; set; }
        public decimal DischargingRate { get; set; }

        public decimal Capacity { get; set; }
        public decimal CurrentState { get; set; }

        public void Charge(PowerTimestamp powerTimestamp, ConcurrencyCalculation calculation)
        {
            var chargeLoad = powerTimestamp.Value;
            if (CurrentState < HighestThreshold)
            {
                if (chargeLoad > ChargingRate)
                {
                    if (ChargingRate >= HighestThreshold - CurrentState)
                    {
                        CurrentState = HighestThreshold; //excess charge chargeload - (HighestThreshold - CurrentState)
                        calculation.PowerToTheNetwork.Add(new PowerTimestamp
                        {
                            Date = powerTimestamp.Date,
                            Value = (chargeLoad - (HighestThreshold - CurrentState))
                        });

                    }
                    else
                    {
                        CurrentState += ChargingRate; //excess charge chargeload - Chargingrate
                        calculation.PowerToTheNetwork.Add(new PowerTimestamp
                        {
                            Date = powerTimestamp.Date,
                            Value = ((chargeLoad - ChargingRate))
                        });

                    }
                }
                else
                {
                    if (chargeLoad >= HighestThreshold - CurrentState)
                    {
                        CurrentState = HighestThreshold; //excess charge chargeload - (HighestThreshold - CurrentState)
                        calculation.PowerToTheNetwork.Add(new PowerTimestamp
                        {
                            Date = powerTimestamp.Date,
                            Value = (chargeLoad - (HighestThreshold - CurrentState))
                        });

                    }
                    else
                    {
                        CurrentState += chargeLoad; //no excess charge
                        calculation.PowerToTheNetwork.Add(new PowerTimestamp
                        {
                            Date = powerTimestamp.Date,
                            Value = 0
                        });

                    }
                }
            }
            calculation.BatteryHistory.Add(new PowerTimestamp { Date = powerTimestamp.Date, Value = CurrentState });
        }

        public void Discharge(PowerTimestamp powerTimestamp, ConcurrencyCalculation calculation)
        {
            var dischargeLoad = powerTimestamp.Value;
            if (CurrentState > LowerstThreshold)
            {
                if (dischargeLoad > DischargingRate)
                {
                    if (DischargingRate >= CurrentState - LowerstThreshold)
                    {
                        CurrentState = LowerstThreshold; //more power needed dischargeLoad - (CurrentState - LowerstThreshold)
                        calculation.PowerFromTheNetwork.Add(new PowerTimestamp
                        {

                        });
                    }
                    else
                    {
                        CurrentState -= DischargingRate; //more power needed dischargeLoad - DischargingRate
                    }
                }
                else
                {
                    if (dischargeLoad >= CurrentState - LowerstThreshold)
                    {
                        CurrentState = LowerstThreshold; //more power needed dischargeLoad - (CurrentState - LowerstThreshold)
                    }
                    else
                    {
                        CurrentState -= dischargeLoad; //no more power needed
                    }

                }
            }
            calculation.BatteryHistory.Add(new PowerTimestamp { Date = powerTimestamp.Date, Value = CurrentState });
        }

    }
}
