using ConferenceRoomBooking.Core.Models;

namespace ConferenceRoomBooking.Core.Interfaces;

public interface IBookingService
{
    Booking CreateBooking(Guid roomId, DateOnly date, TimeOnly startTime, TimeOnly endTime, List<Guid> serviceIds);
    Booking? GetBookingById(Guid bookingId);
    IEnumerable<Booking> GetAllBookings();
}