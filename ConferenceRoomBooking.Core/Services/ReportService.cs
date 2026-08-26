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
}
