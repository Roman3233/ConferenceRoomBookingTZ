using ConferenceRoomBooking.Core.Models;

namespace ConferenceRoomBooking.Core.Interfaces;

public interface IBookingRepository
{
    Booking? GetById(Guid id);
    IEnumerable<Booking> GetAll();
    Booking Add(Booking booking);
    void Update(Booking booking);
    bool Delete(Guid id);
    // Повертає всі бронювання конкретного залу на конкретну дату
    IEnumerable<Booking> GetByRoomAndDate(Guid roomId, DateOnly date);
}