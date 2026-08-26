namespace ConferenceRoomBooking.Core.Interfaces;

public interface IBookingAvailabilityChecker
{
    bool HasConflictingBooking(Guid roomId, DateOnly date, TimeOnly startTime, TimeOnly endTime);
}