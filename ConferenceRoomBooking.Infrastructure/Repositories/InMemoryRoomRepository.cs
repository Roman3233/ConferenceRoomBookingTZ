using ConferenceRoomBooking.Core.Interfaces;
using ConferenceRoomBooking.Core.Models;

namespace ConferenceRoomBooking.Infrastructure.Repositories;

public class InMemoryRoomRepository : IRoomRepository
{
    private readonly List<Room> _rooms = new();
    private readonly object _lock = new();

    public Room? GetById(Guid id)
    {
        lock(_lock)
        {
            return _rooms.FirstOrDefault(r => r.Id == id);
        }
    }

    public IEnumerable<Room> GetAll()
    {
        lock(_lock)
        {
            return _rooms.ToList();
        }
    }

    public Room Add(Room room)
    {
        lock(_lock)
        {
            room.Id = Guid.NewGuid();
            _rooms.Add(room);
            return room;
        }
    }

    public void Update(Room room)
    {
        lock(_lock)
        {
            var existing = GetById(room.Id);
            if (existing is null)
            {
                throw new InvalidOperationException($"Room with id {room.Id} not found.");
            }

            _rooms.Remove(existing);
            _rooms.Add(room);
        }
    }

    public bool Delete(Guid id)
    {
        lock(_lock)
        {
            var room = GetById(id);
            if (room is null)
            {
                return false;
            }

            return _rooms.Remove(room);
        }
    }
}