namespace ConferenceRoomBooking.Core.Interfaces;

public interface IPricingCalculator
{
    decimal CalculateRoomCost(decimal basePricePerHour, TimeOnly startTime, TimeOnly endTime);
}