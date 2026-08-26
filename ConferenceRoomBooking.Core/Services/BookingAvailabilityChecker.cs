using ConferenceRoomBooking.Core.Interfaces;

namespace ConferenceRoomBooking.Core.Services;
// Клас для перевірки доступності бронювань та уникнення дублювання коду в сервісах BookingService та RoomService.
public class BookingAvailabilityChecker : IBookingAvailabilityChecker
{
    private readonly IBookingRepository _bookingRepository;

    public BookingAvailabilityChecker(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }
    public bool HasConflictingBooking(Guid roomId, DateOnly date, TimeOnly startTime, TimeOnly endTime)
    {
        var bookingsOnDate = _bookingRepository.GetByRoomAndDate(roomId, date);

        return bookingsOnDate.Any(b => IntervalsOverlap(startTime, endTime, b.StartTime, b.EndTime));
    }
    private static bool IntervalsOverlap(TimeOnly newStart, TimeOnly newEnd, TimeOnly existingStart, TimeOnly existingEnd)
    {
        return newEnd > existingStart && newStart < existingEnd;
    }
}