using ConferenceRoomBooking.Core.Models;

namespace ConferenceRoomBooking.Core.Interfaces;

public interface IRoomRepository
{
    Room? GetById(Guid id);
    IEnumerable<Room> GetAll();
    Room Add(Room room);
    void Update(Room room);
    bool Delete(Guid id);
}