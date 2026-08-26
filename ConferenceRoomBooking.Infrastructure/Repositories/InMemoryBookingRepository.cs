using ConferenceRoomBooking.Core.Interfaces;
using ConferenceRoomBooking.Core.Models;

namespace ConferenceRoomBooking.Infrastructure.Repositories;

public class InMemoryBookingRepository : IBookingRepository
{
    private readonly List<Booking> _bookings = new();
    private readonly object _lock = new();

    public Booking Add(Booking booking)
    {
        lock (_lock)
        {
            booking.Id = Guid.NewGuid();
            _bookings.Add(booking);
            return booking;
        }
    }

    public void Update(Booking booking)
    {
        lock (_lock)
        {
            var existing = _bookings.FirstOrDefault(b => b.Id == booking.Id);
            if (existing is null)
            {
                throw new InvalidOperationException($"Booking with id {booking.Id} not found.");
            }

            _bookings.Remove(existing);
            _bookings.Add(booking);
        }
    }

    public bool Delete(Guid id)
    {
        lock (_lock)
        {
            var booking = _bookings.FirstOrDefault(b => b.Id == id);
            if (booking is null)
            {
                return false;
            }

            return _bookings.Remove(booking);
        }
    }
    public Booking? GetById(Guid id)
    {
        lock (_lock)
        {
            return _bookings.FirstOrDefault(b => b.Id == id);
        }
    }

    public IEnumerable<Booking> GetAll()
    {
        lock (_lock)
        {
            return _bookings.ToList();
        }
    }
    // Отримання всіх бронювань для конкретного залу на певну дату
    public IEnumerable<Booking> GetByRoomAndDate(Guid roomId, DateOnly date)
    {
        lock (_lock)
        {
            return _bookings.Where(b => b.RoomId == roomId && b.Date == date).ToList();
        }
    }
}