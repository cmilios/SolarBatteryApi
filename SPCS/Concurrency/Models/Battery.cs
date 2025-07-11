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
            LowestThreshold = lowestThreshold;
            HighestThreshold = highestThreshold;
            Capacity = capacity;
            CurrentState = initialState;
            ChargingRate = chargingRate;
            DischargingRate = dischargingRate;
        }

        public decimal InitialState { get; set; }
        public decimal LowestThreshold { get; set; }
        public decimal HighestThreshold { get; set; }
        public decimal ChargingRate { get; set; }
        public decimal DischargingRate { get; set; }

        public decimal Capacity { get; set; }
        public decimal CurrentState { get; set; }

        public decimal ChangeCurrentCharge(List<PowerTimestamp> batteryHistory, PowerTimestamp chargeLoad)
        {
            var energy = chargeLoad.Value;
            if (energy > 0)
            {
                decimal availableCapacity = Math.Max(HighestThreshold - CurrentState, 0);
                decimal chargeAmount = Math.Min(energy, Math.Min(ChargingRate, availableCapacity));
                CurrentState += chargeAmount;
                batteryHistory.Add(new PowerTimestamp { Date = chargeLoad.Date, Value = CurrentState, Type = Enum.PowerTimestampType.BatteryHistory });

                return chargeAmount;
            }
            else if (energy < 0)
            {
                decimal availableEnergy = Math.Max(CurrentState - LowestThreshold, 0);
                decimal dischargeAmount = Math.Min(-energy, Math.Min(DischargingRate, availableEnergy));
                CurrentState -= dischargeAmount;
                batteryHistory.Add(new PowerTimestamp { Date = chargeLoad.Date, Value = CurrentState, Type = Enum.PowerTimestampType.BatteryHistory });

                return -dischargeAmount;
            }
            batteryHistory.Add(new PowerTimestamp { Date = chargeLoad.Date, Value = CurrentState, Type = Enum.PowerTimestampType.BatteryHistory });
            return 0;

        }

    }
}
