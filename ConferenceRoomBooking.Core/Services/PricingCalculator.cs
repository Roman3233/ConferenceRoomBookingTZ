using ConferenceRoomBooking.Core.Interfaces;

namespace ConferenceRoomBooking.Core.Services;

public class PricingCalculator : IPricingCalculator
{
    // Часові зони: початок, кінець, коефіцієнт
    private static readonly (TimeOnly Start, TimeOnly End, decimal Multiplier)[] PricingZones =
    [
        (new TimeOnly(6, 0),  new TimeOnly(9, 0),  0.9m),   // ранкові, -10%
        (new TimeOnly(9, 0),  new TimeOnly(12, 0), 1.0m),   // стандартні
        (new TimeOnly(12, 0), new TimeOnly(14, 0), 1.15m),  // пікові, +15%
        (new TimeOnly(14, 0), new TimeOnly(18, 0), 1.0m),   // стандартні
        (new TimeOnly(18, 0), new TimeOnly(23, 0), 0.8m),   // вечірні, -20%
    ];

    public decimal CalculateRoomCost(decimal basePricePerHour, TimeOnly startTime, TimeOnly endTime)
    {
        if (startTime >= endTime)
        {
            throw new ArgumentException("Start time must be before end time.");
        }

        decimal totalCost = 0;
        TimeOnly currentTime = startTime;

        while (currentTime < endTime)
        {
            var zone = FindZone(currentTime);

            // Відрізок обрізаємо по межі зони або по кінцю бронювання - що настане раніше
            TimeOnly segmentEnd = zone.End < endTime ? zone.End : endTime;

            decimal segmentHours = (decimal)(segmentEnd - currentTime).TotalHours;
            totalCost += basePricePerHour * zone.Multiplier * segmentHours;

            currentTime = segmentEnd;
        }

        return totalCost;
    }

    private static (TimeOnly Start, TimeOnly End, decimal Multiplier) FindZone(TimeOnly time)
    {
        foreach (var zone in PricingZones)
        {
            if (time >= zone.Start && time < zone.End)
            {
                return zone;
            }
        }

        throw new InvalidOperationException("Booking outside working hours (06:00-23:00).");
    }
}