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

        public decimal ChangeCurrentCharge(List<PowerTimestamp> batteryHistory, PowerTimestamp powerTimestamp)
        {
            var power = powerTimestamp.Value;
            // Determine the actual amount to transfer, limited by max transfer rate
            decimal transferAmount = Math.Clamp(power, -DischargingRate, ChargingRate);
            // Calculate the new charge after applying the transfer amount
            decimal newCharge = CurrentState + transferAmount;

            if (newCharge < 0)
            {
                newCharge = Math.Max(newCharge, LowestThreshold);
            }
            // Enforce 10% and 90% charge limits
            if (CurrentState >= LowestThreshold)
            {
                // Clamp within the min and max charge thresholds (10% and 90%)
                newCharge = Math.Clamp(newCharge, LowestThreshold, HighestThreshold);
            }
            else
            {
                // Clamp only by the maximum limit if starting below the min threshold
                newCharge = Math.Min(newCharge, HighestThreshold);
            }

            // Calculate the actual amount of charge/discharge applied
            decimal actualAmountTransferred = newCharge - CurrentState;

            // Update the current charge
            CurrentState = newCharge;
            batteryHistory.Add(new PowerTimestamp { Date = powerTimestamp.Date, Value = CurrentState });
            // Return the actual amount transferred
            return actualAmountTransferred;
        }

    }
}
