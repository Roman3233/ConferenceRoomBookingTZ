using System;
using System.Collections.Generic;

namespace ConferenceRoomBooking.Core.Interfaces;

public interface IReportService
{
    (decimal TotalRevenue, decimal RoomRevenue, decimal ServiceRevenue) GetRevenueReport(DateOnly startDate, DateOnly endDate);
    IEnumerable<(Guid RoomId, string RoomName, double TotalHoursBooked, double OccupancyRate)> GetOccupancyReport(DateOnly startDate, DateOnly endDate);
}
