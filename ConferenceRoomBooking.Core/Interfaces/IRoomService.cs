using ConferenceRoomBooking.Core.Models;

namespace ConferenceRoomBooking.Core.Interfaces;

public interface IRoomService
{
    Room CreateRoom(string name, int capacity, decimal basePricePerHour, List<Guid> serviceIds);
    void UpdateRoom(Guid roomId, string? name, int? capacity, decimal? basePricePerHour, List<Guid>? serviceIds);
    void DeleteRoom(Guid roomId);
    IEnumerable<Room> FindAvailableRooms(DateOnly date, TimeOnly startTime, TimeOnly endTime, int minCapacity);
    Room? GetRoomById(Guid roomId);
    IEnumerable<Room> GetAllRooms();
}