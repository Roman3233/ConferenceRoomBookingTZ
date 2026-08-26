using System;
using System.Collections.Generic;
using System.Linq;
using ConferenceRoomBooking.Core.Interfaces;
using ConferenceRoomBooking.Core.Models;

namespace ConferenceRoomBooking.Core.Services;

public class ReportService : IReportService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IRoomRepository _roomRepository;

    public ReportService(IBookingRepository bookingRepository, IRoomRepository roomRepository)
    {
        _bookingRepository = bookingRepository;
        _roomRepository = roomRepository;
    }
    // Звіт про доходи
    public (decimal TotalRevenue, decimal RoomRevenue, decimal ServiceRevenue) GetRevenueReport(DateOnly startDate, DateOnly endDate)
    {
        var bookings = _bookingRepository.GetAll()
            .Where(b => b.Date >= startDate && b.Date <= endDate)
            .ToList();

        var total = bookings.Sum(b => b.TotalPrice);
        var rooms = bookings.Sum(b => b.RoomCostAtBooking);
        var services = total - rooms;

        return (total, rooms, services);
    }

    // Звіт про завантаженість залів
    public IEnumerable<(Guid RoomId, string RoomName, double TotalHoursBooked, double OccupancyRate)> GetOccupancyReport(DateOnly startDate, DateOnly endDate)
    {
        var bookings = _bookingRepository.GetAll()
            .Where(b => b.Date >= startDate && b.Date <= endDate)
            .ToList();

        var rooms = _roomRepository.GetAll();
        var report = new List<(Guid RoomId, string RoomName, double TotalHoursBooked, double OccupancyRate)>();

        // Робочий день триває 15 годин
        int daysCount = endDate.DayNumber - startDate.DayNumber + 1;
        double totalAvailableHoursPerRoom = daysCount * 15.0;

        foreach (var room in rooms)
        {
            var roomBookings = bookings.Where(b => b.RoomId == room.Id).ToList();
            double totalHoursBooked = 0;

            foreach (var booking in roomBookings)
            {
                var duration = (booking.EndTime - booking.StartTime).TotalHours;
                totalHoursBooked += duration;
            }

            double occupancyRate = totalAvailableHoursPerRoom > 0 
                ? (totalHoursBooked / totalAvailableHoursPerRoom) * 100 
                : 0;

            report.Add((room.Id, room.Name, Math.Round(totalHoursBooked, 2), Math.Round(occupancyRate, 2)));
        }

        return report;
    }

}
