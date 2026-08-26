using ConferenceRoomBooking.Core.Interfaces;
using ConferenceRoomBooking.Core.Models;

namespace ConferenceRoomBooking.Core.Services;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepository;
    private readonly IBookingAvailabilityChecker _bookingAvailabilityChecker;

    public RoomService(IRoomRepository roomRepository, IBookingAvailabilityChecker bookingAvailabilityChecker)
    {
        _roomRepository = roomRepository;
        _bookingAvailabilityChecker = bookingAvailabilityChecker;
    }

    public Room CreateRoom(string name, int capacity, decimal basePricePerHour, List<Guid> serviceIds)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Room name cannot be empty.", nameof(name));
        }

        if (capacity <= 0)
        {
            throw new ArgumentException("Capacity must be greater than zero.", nameof(capacity));
        }

        if (basePricePerHour <= 0)
        {
            throw new ArgumentException("Base price must be greater than zero.", nameof(basePricePerHour));
        }

        var room = new Room
        {
            Name = name,
            Capacity = capacity,
            BasePricePerHour = basePricePerHour,
            AvailableServiceIds = serviceIds
        };

        return _roomRepository.Add(room);
    }

    public void UpdateRoom(Guid roomId, string? name, int? capacity, decimal? basePricePerHour, List<Guid>? serviceIds)
    {
        var room = _roomRepository.GetById(roomId);
        if (room is null)
        {
            throw new InvalidOperationException($"Room with id {roomId} not found.");
        }

        if (capacity is not null && capacity.Value <= 0)
        {
            throw new ArgumentException("Capacity must be greater than zero.", nameof(capacity));
        }
        if (basePricePerHour is not null && basePricePerHour.Value <= 0)
        {
            throw new ArgumentException("Base price must be greater than zero.", nameof(basePricePerHour));
        }
        // Якщо поля не null, то вони будуть оновлені
        if (name is not null)
        {
            room.Name = name;
        }

        if (capacity is not null)
        {
            room.Capacity = capacity.Value;
        }

        if (basePricePerHour is not null)
        {
            room.BasePricePerHour = basePricePerHour.Value;
        }

        if (serviceIds is not null)
        {
            room.AvailableServiceIds = serviceIds;
        }

        _roomRepository.Update(room);
    }

    public void DeleteRoom(Guid roomId)
    {
        var deleted = _roomRepository.Delete(roomId);
        if (!deleted)
        {
            throw new InvalidOperationException($"Room with id {roomId} not found.");
        }
    }
    // Пошук вільних залів
    public IEnumerable<Room> FindAvailableRooms(DateOnly date, TimeOnly startTime, TimeOnly endTime, int minCapacity)
    {
        var candidateRooms = _roomRepository.GetAll().Where(r => r.Capacity >= minCapacity);

        return candidateRooms.Where(room => !_bookingAvailabilityChecker.HasConflictingBooking(room.Id, date, startTime, endTime));
    }

    public Room? GetRoomById(Guid roomId)
    {
        return _roomRepository.GetById(roomId);
    }

    public IEnumerable<Room> GetAllRooms()
    {
        return _roomRepository.GetAll();
    }
}